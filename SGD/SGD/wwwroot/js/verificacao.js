class DraggableTable {
    constructor() {
        this.tableBody = document.getElementById('tableBody');
        this.draggedElement = null;
        this.init();
    }

    init() {
        this.tableBody.addEventListener('dragstart', this.handleDragStart.bind(this));
        this.tableBody.addEventListener('dragover', this.handleDragOver.bind(this));
        this.tableBody.addEventListener('drop', this.handleDrop.bind(this));
        this.tableBody.addEventListener('dragend', this.handleDragEnd.bind(this));
        this.tableBody.addEventListener('dragenter', this.handleDragEnter.bind(this));
        this.tableBody.addEventListener('dragleave', this.handleDragLeave.bind(this));
    }

    handleDragStart(e) {
        if (e.target.closest('.table-row')) {
            this.draggedElement = e.target.closest('.table-row');
            // Tailwind: Opacidade para indicar que está sendo arrastado
            this.draggedElement.classList.add('opacity-50', 'bg-gray-100');
            e.dataTransfer.effectAllowed = 'move';
            e.dataTransfer.setData('text/html', '');
        }
    }

    handleDragOver(e) {
        e.preventDefault();
        e.dataTransfer.dropEffect = 'move';
    }

    handleDragEnter(e) {
        const row = e.target.closest('.table-row');
        if (row && row !== this.draggedElement) {
            // Tailwind: Indicador visual de onde vai cair (borda azul pontilhada)
            row.classList.add('border-t-2', 'border-blue-400', 'border-dashed');
        }
    }

    handleDragLeave(e) {
        const row = e.target.closest('.table-row');
        if (row) {
            row.classList.remove('border-t-2', 'border-blue-400', 'border-dashed');
        }
    }

    handleDrop(e) {
        e.preventDefault();
        const dropTarget = e.target.closest('.table-row');

        if (dropTarget && this.draggedElement && dropTarget !== this.draggedElement) {
            if (this.draggedElement.compareDocumentPosition(dropTarget) & Node.DOCUMENT_POSITION_FOLLOWING) {
                dropTarget.parentNode.insertBefore(this.draggedElement, dropTarget.nextSibling);
            } else {
                dropTarget.parentNode.insertBefore(this.draggedElement, dropTarget);
            }

            // Calcular nova posição
            const allRows = Array.from(this.tableBody.children);
            const newIndex = allRows.indexOf(this.draggedElement);

            // Atualiza índice global se necessário
            selectedRowIndex = newIndex;

            // Chamar API
            MoveImagem(this.draggedElement.getAttribute('data-img'), newIndex);
        }
        this.clearDragStyles();
    }

    handleDragEnd() {
        this.clearDragStyles();
    }

    clearDragStyles() {
        const rows = this.tableBody.querySelectorAll('.table-row');
        rows.forEach(row => {
            row.classList.remove('opacity-50', 'bg-gray-100', 'border-t-2', 'border-blue-400', 'border-dashed');
        });
        this.draggedElement = null;
    }
}

// --- Variáveis Globais ---
let selectedRowIndex = null;
let fetchTimeout = null;
let keyHeld = false;
let firstrow = null;
let scale = 1, tx = 0, ty = 0, dragging = false, sx = 0, sy = 0, stx = 0, sty = 0;
let idLote;

// Elementos DOM
const viewport = document.getElementById("viewport");
const imgElement = document.getElementById("previewImg");
const placeholderElement = document.getElementById("empty-placeholder");
const btnImgRuim = document.getElementById("btn-img-ruim");
const docInput = document.getElementById('docNumero');
const docErro = document.getElementById('docErro');
const btnSalvarDoc = document.getElementById('btnSalvarDoc');

document.addEventListener('DOMContentLoaded', () => {
    idLote = document.getElementById('idlote').value;

    // Inicializa Drag and Drop
    new DraggableTable();

    // Seleciona a primeira linha automaticamente se existir
    const tableBody = document.getElementById('tableBody');
    const firstRow = tableBody.querySelector('.table-row');
    if (firstRow) {
        highlightRow(firstRow);
    }
});

// --- Lógica de Seleção de Linha ---
const tableBody = document.getElementById('tableBody');

// Event Delegation para cliques na tabela
tableBody.addEventListener('click', function (e) {
    const row = e.target.closest('.table-row');
    if (row) {
        highlightRow(row);
    }
});

function highlightRow(row) {
    const tableBody = document.getElementById('tableBody');

    // 1. Remove estilos de seleção anteriores (Tailwind)
    Array.from(tableBody.children).forEach(r => {
        r.classList.remove('bg-blue-100', 'border-l-4', 'border-blue-500');
    });

    // 2. Adiciona estilos na nova linha
    row.classList.add('bg-blue-100', 'border-l-4', 'border-blue-500');

    // 3. Atualiza índice global
    selectedRowIndex = Array.from(tableBody.children).indexOf(row);

    // 4. Carrega Imagem
    const imgId = row.getAttribute('data-img');
    carregarImagem(imgId);
}

function carregarImagem(imgId) {
    if (!imgId) return;

    // Mostra loading ou algo visual se desejar
    fetch("/Verify/MostrarImagem", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(imgId)
    })
        .then(res => res.blob())
        .then(blob => {
            const url = URL.createObjectURL(blob);
            imgElement.src = url;

            // UI: Troca placeholder pela imagem
            placeholderElement.classList.add('hidden');
            imgElement.classList.remove('hidden');

            // Reseta Zoom ao trocar imagem
            scale = 1; tx = 0; ty = 0;
            applyZoom();
        });
}

// --- Ações de Botões e Teclado ---

// Botão "Imagem Ruim"
btnImgRuim.addEventListener('click', () => {
    if (selectedRowIndex === null) return;
    atualizarStatusImagem("RUIM");
});

function atualizarStatusImagem(status) {
    const row = tableBody.children[selectedRowIndex];
    if (!row) return;

    const dto = {
        imagem: row.getAttribute("data-img"),
        situacao: status
    };

    fetch("/Verify/AtualizaImagem", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(dto)
    }).then(res => {
        if (res.ok) {
            atualizarVisualLinha(row, status);
        } else {
            showToast("Erro ao atualizar imagem", "error");
        }
    });
}

function atualizarVisualLinha(row, status) {
    // Coluna de Status é a 3ª (index 2)
    const statusCell = row.children[2];
    statusCell.textContent = status;

    // Remove classes antigas de cor
    row.classList.remove('bg-red-50', 'hover:bg-red-100');
    statusCell.className = 'px-4 py-3 text-right font-bold'; // Reset base

    if (status === "RUIM" || status === "DEL") {
        row.classList.add('bg-red-50', 'hover:bg-red-100'); // Fundo vermelho suave
        statusCell.classList.add('text-red-600');
    } else if (status === "OK") {
        statusCell.classList.add('text-green-600');
    } else {
        statusCell.classList.add('text-gray-500');
    }
}


// --- Navegação por Teclado ---
document.addEventListener("keydown", function (event) {
    const rows = document.getElementById("tableBody").children;
    if (selectedRowIndex === null || rows.length === 0) return;

    // Navegação
    if (event.key === "ArrowDown") {
        event.preventDefault();
        if (selectedRowIndex < rows.length - 1) highlightRow(rows[selectedRowIndex + 1]);
    } else if (event.key === "ArrowUp") {
        event.preventDefault();
        if (selectedRowIndex > 0) highlightRow(rows[selectedRowIndex - 1]);
    }

    // Ações Rápidas (Enter / Delete)
    if (event.key === "Enter" || event.key === "Delete") {
        // Lógica para aplicar na linha anterior se digitar rápido
        let targetIndex = selectedRowIndex;
        if (!keyHeld && selectedRowIndex > 0) {
            // Opcional: Se quiser comportamento estilo "linha de produção"
            // targetIndex = selectedRowIndex - 1; 
        }

        const status = event.key === "Enter" ? "OK" : "DEL";

        // Impede deletar se tiver documento
        const row = rows[targetIndex];
        if (status === "DEL" && row.getAttribute('data-doc')) {
            // showToast("Não pode deletar imagem com documento", "warning"); 
            // return; 
        }

        const dto = {
            imagem: row.getAttribute("data-img"),
            situacao: status
        };

        fetch("/Verify/AtualizaImagem", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(dto)
        }).then(res => {
            if (res.ok) {
                atualizarVisualLinha(row, status);
                // Avança para o próximo automaticamente
                if (selectedRowIndex < rows.length - 1) {
                    highlightRow(rows[selectedRowIndex + 1]);
                }
            }
        });
    }
});


// --- Lógica do Modal (Inserir Documento) ---

// Funções para controlar o modal Tailwind (substituindo Bootstrap)
function toggleModal(show) {
    const modal = document.getElementById('mdlDocumento');
    if (show) {
        modal.classList.remove('hidden');
        setTimeout(() => docInput.focus(), 100);
    } else {
        modal.classList.add('hidden');
        docInput.value = '';
        docErro.classList.add('hidden');
        btnSalvarDoc.disabled = true;
    }
}

// Botão "Novo Doc" da Toolbar
document.getElementById('inseredoc').addEventListener('click', () => {
    if (selectedRowIndex === null) {
        alert("Selecione uma imagem primeiro.");
        return;
    }
    toggleModal(true);
});

// Validação Input
docInput.addEventListener('input', () => {
    docInput.value = docInput.value.replace(/\D/g, '').slice(0, 10);
    const isValid = docInput.value.length > 0 && docInput.value.length <= 10;

    btnSalvarDoc.disabled = !isValid;
    if (!isValid && docInput.value.length > 0) docErro.classList.remove('hidden');
    else docErro.classList.add('hidden');
});

// Salvar Documento
btnSalvarDoc.addEventListener('click', async () => {
    const row = tableBody.children[selectedRowIndex];
    const imagemId = row.getAttribute('data-img');
    const valorDoc = docInput.value.trim();

    const dto = {
        id: imagemId,
        documento: valorDoc,
        LoteId: idLote,
        remover: "0" // Lógica de remover pode ser adicionada se necessário
    };

    const res = await fetch("/Verify/InsereDocumento", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(dto)
    });

    if (res.ok) {
        // Atualiza UI
        row.setAttribute('data-doc', valorDoc);
        // Coluna 1 é Doc (index 0)
        row.children[0].textContent = valorDoc;
        toggleModal(false);
    } else {
        alert("Erro ao salvar documento");
    }
});


// --- Upload de Imagem (TIF) ---
const inputTif = document.getElementById('fileTif');
inputTif.addEventListener('change', async () => {
    const formData = new FormData();
    formData.append("idLote", idLote);
    formData.append("posicao", selectedRowIndex !== null ? selectedRowIndex : 0);

    Array.from(inputTif.files || [])
        .filter(f => /\.tiff?$/i.test(f.name))
        .forEach(f => formData.append("files", f));

    const resp = await fetch('/Verify/InsereImagem', { method: 'POST', body: formData });

    if (resp.ok) {
        // Recarrega a página ou insere manualmente na DOM
        // Para simplificar, recarregar é mais seguro para garantir IDs
        location.reload();
    } else {
        alert('Falha no upload');
    }
    inputTif.value = '';
});


// --- Zoom e Pan (Mantido Lógica Original) ---
viewport.addEventListener("wheel", e => {
    if (e.target !== imgElement) return;
    e.preventDefault();
    const rect = viewport.getBoundingClientRect();
    const px = e.clientX - rect.left;
    const py = e.clientY - rect.top;
    const oldScale = scale;
    const zoom = Math.exp(-e.deltaY * 0.0005);
    scale = Math.min(10, Math.max(0.1, scale * zoom));
    const ix = (px - tx) / oldScale;
    const iy = (py - ty) / oldScale;
    tx = px - ix * scale;
    ty = py - iy * scale;
    applyZoom();
}, { passive: false });

viewport.addEventListener("mousedown", e => {
    if (e.button !== 0 || !imgElement.src) return;
    dragging = true; sx = e.clientX; sy = e.clientY; stx = tx; sty = ty;
    viewport.classList.add("cursor-grabbing");
});

window.addEventListener("mousemove", e => {
    if (!dragging) return;
    tx = stx + (e.clientX - sx);
    ty = sty + (e.clientY - sy);
    applyZoom();
});

window.addEventListener("mouseup", () => {
    dragging = false;
    viewport.classList.remove("cursor-grabbing");
});

imgElement.ondragstart = e => e.preventDefault();

function applyZoom() {
    imgElement.style.transform = `translate(${tx}px,${ty}px) scale(${scale})`;
}

// --- Funções Auxiliares ---
function MoveImagem(imagem, posi) {
    const dto = { id: imagem, posicao: posi };
    fetch("/Verify/MoveImagem", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(dto)
    }).catch(err => console.error(err));
}