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
    this.draggedElement.classList.add('dragging');
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
        row.classList.add('drag-over');
                }
            }

    handleDragLeave(e) {
                const row = e.target.closest('.table-row');
    if (row) {
        row.classList.remove('drag-over');
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

    // calcular nova posição real após a mudança
    const allRows = Array.from(this.tableBody.children);
    const newIndex = allRows.indexOf(this.draggedElement);

    // chamar API com posição correta
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
        row.classList.remove('dragging', 'drag-over');
                });
    this.draggedElement = null;
            }
        }
    let selectedRowIndex = null;
    let fetchTimeout = null;
    let keyHeld = false;
    let firstrow = null;
    let scale = 1, tx = 0, ty = 0, dragging = false, sx = 0, sy = 0, stx = 0, sty = 0;
    let idlote;
    const btnInsere = document.getElementById('inseredoc');
    document.addEventListener('DOMContentLoaded', () => {
                const tableBody = document.getElementById('tableBody');
    idLote = document.getElementById('ilote').value;

    // Seleciona e clica na primeira linha
    const firstRow = tableBody.querySelector('.table-row');
    if (firstRow) {
        firstRow.click();
                }

                // Percorre todas as linhas da tabela
                Array.from(tableBody.children).forEach((linha, i) => {
                const status = linha.getAttribute("data-img-st");

    switch (status) {
                    case "OK":
    linha.classList.add("image-ok");
    break;
    case "DEL":
    linha.classList.add("image-del");
    break;
    case "RUIM":
    linha.classList.add("image-ruim");
    break;
                }
                });
            });
    tableBody.addEventListener('click', function (e) {
        const tableBody = document.getElementById('tableBody');
    const previewImg = document.getElementById('previewImg');
    const row = e.target.closest('.table-row');
    if (row) {

            const img = row.getAttribute('data-img');
    const lote = document.getElementById('idlote');
                tableBody.querySelectorAll('.table-row').forEach(r => r.classList.remove('selected'));

    // Adiciona a classe na linha clicada
    row.classList.add('selected');

    selectedRowIndex = Array.from(tableBody.children).indexOf(row);


    fetch("/Verify/MostrarImagem", {
        method: "POST",
    headers: {
        "Content-Type": "application/json"
                },
    body: JSON.stringify(img)
                })
                .then(res => res.blob())
                .then(blob => {
        previewImg.src = URL.createObjectURL(blob);
                });
        }
    });
    document.addEventListener('DOMContentLoaded', function() {
        new DraggableTable();
        });

    const viewport = document.getElementById("viewport");
    const img = document.getElementById("previewImg");
    const btnImgRuim = document.getElementById("btn-img-ruim");
    const docInput = document.getElementById('docNumero');
    const docErro = document.getElementById('docErro');
    const btnSalvarDoc = document.getElementById('btnSalvarDoc');

    btnImgRuim.addEventListener('click', () => {
            const tableBody = document.getElementById("tableBody");
    const row = tableBody.children[selectedRowIndex];

    const dto = {
        imagem: row.getAttribute("data-img"),
    situacao: "RUIM"
            };

    fetch("/Verify/AtualizaImagem", {
        method: "POST",
    headers: {"Content-Type": "application/json" },
    body: JSON.stringify(dto)
            }).then(res => {
                if (res.ok) {
        row.className = "table-row"; // reseta
    row.classList.add("image-ruim");
                } else {
        showMensagem("Erro ao atualizar imagem", "erro");
    return;
                }
            });
        });
    docInput.addEventListener('keydown', soDigitos);
    docInput.addEventListener('input', () => {
        // garante só dígitos e tamanho 10
        docInput.value = docInput.value.replace(/\D/g, '').slice(0, 10);
    validaDoc();
        });

    btnSalvarDoc.addEventListener('click', async () => {
        if (!validaDoc()) {
            return;
        }        

        const tableBody = document.getElementById('tableBody');
        if (selectedRowIndex == null) {alert('Selecione uma imagem.'); return; }

        const row = tableBody.children[selectedRowIndex];
        if (row.classList.contains('image-del')) {
                    const m = bootstrap.Modal.getInstance(document.getElementById('mdlDocumento'))
        || new bootstrap.Modal(document.getElementById('mdlDocumento'));
        m.hide();
        showMensagem("Não é possivel inserir documento em imagens deletas",true);
        return;
                }

        const imagemId = row.getAttribute('data-img');


        const remover = docInput.value.trim() === '';
        const dto = {
            id: imagemId,
            documento: remover? row.children[0].outerText : docInput.value.trim(),
            LoteId: idLote,            
            remover: remover? "1" : "0"
        };

        const res = await fetch("/Verify/InsereDocumento", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(dto)
        });
        const msg = await res.text();
        if (res.ok) {
            showMensagem(msg);
            if (remover) {
                dto.documento = "";
            }
            row.setAttribute('data-doc', dto.documento);
            row.children[0].textContent = dto.documento || '';

        } else {
            showMensagem(msg);
        }

       
        // fechar modal
        if (window.bootstrap) {
                    const m = bootstrap.Modal.getInstance(document.getElementById('mdlDocumento'))
        || new bootstrap.Modal(document.getElementById('mdlDocumento'));
        m.hide();
                }

        // limpar campo
        docInput.value = '';
    });        



    document.getElementById('mdlDocumento').addEventListener('show.bs.modal', () => {
        docInput.value = '';
    btnSalvarDoc.disabled = false;
    docErro.style.display = 'none';
            setTimeout(() => docInput.focus(), 150);
        });
    document.addEventListener("keydown", function (event) {
            if (selectedRowIndex !== null) {
                // Marca que foi pressionado Enter na linha atual
                if (!keyHeld && event.key === "Enter" || event.key === "Delete") {
        keyHeld = true;
    firstrow = selectedRowIndex;
                }

    if (
    (event.key === "ArrowDown" || event.key === "Enter" || event.key === "Delete") &&
    selectedRowIndex < document.getElementById("tableBody").children.length - 1
    ) {
        updateSelection(selectedRowIndex + 1);
                }

                if (event.key === "ArrowUp" && selectedRowIndex > 0) {
        updateSelection(selectedRowIndex - 1);
                }
            }
        });
    document.addEventListener("keyup", function (event) {
            if (selectedRowIndex === null) return;

    const tableBody = document.getElementById("tableBody");
    const previewImg = document.getElementById("previewImg");
    const row = tableBody.children[selectedRowIndex];

            // Mapeamento: tecla → {status, classe}
    const actions = {
        "Enter": {status: "OK", class: "image-ok", applyToPrev: true },
    "Delete": {status: "DEL", class: "image-del", applyToPrev: true }
            };

    const action = actions[event.key];
    if (action) {
                // ✅ Só executa se a tecla foi solta na mesma linha onde foi pressionada
                if (firstrow === (selectedRowIndex - 1)) {
        let targetRow = row;


                    // Se for necessário aplicar na linha anterior (caso Enter)
                    if (action.applyToPrev && selectedRowIndex > 0) {
        targetRow = tableBody.children[selectedRowIndex - 1];
    if (targetRow.getAttribute('data-doc') != '' & action.status === "DEL") {
        showMensagem("Não é possível deletar imagens marcadas como etiqueta", "erro");
        //showAlert("Não é possível deletar imagens marcadas como etiqueta");
    return;
                        }
                    }

    const dto = {
        imagem: targetRow.getAttribute("data-img"),
    situacao: action.status
                    };

    fetch("/Verify/AtualizaImagem", {
        method: "POST",
    headers: {"Content-Type": "application/json" },
    body: JSON.stringify(dto)
                    }).then(res => {
                        if (res.ok) {
        targetRow.className = "table-row"; // reseta
    targetRow.classList.add(action.class);
                        }
                    });
                }
            }

    // --- Mostrar Imagem (só no Enter)
    if (event.key === "Enter" || event.key === "ArrowDown" || event.key === "ArrowUp") {
                const img = row.getAttribute("data-img");

    if (fetchTimeout) clearTimeout(fetchTimeout);
                fetchTimeout = setTimeout(() => {
        fetch("/Verify/MostrarImagem", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(img)
        })
            .then(res => res.blob())
            .then(blob => {
                previewImg.src = URL.createObjectURL(blob);
            });
                }, 50);


            }
    keyHeld = false;
        });        
    viewport.addEventListener("wheel", e => {
            if (e.target !== img) return;  // só reage se estiver na imagem
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

    apply();
        }, {passive: false });
    viewport.addEventListener("mousedown", e => {
            if (e.button !== 0 || !img.src) return;
    dragging = true; sx = e.clientX; sy = e.clientY; stx = tx; sty = ty;
    viewport.classList.add("grabbing");
        });
    window.addEventListener("mousemove", e => {
            if (!dragging) return;
    tx = stx + (e.clientX - sx);
    ty = sty + (e.clientY - sy);
    apply();
        });
    window.addEventListener("mouseup", () => {
        dragging = false;
    viewport.classList.remove("grabbing"); 
        });
    img.ondragstart = e => e.preventDefault();

    function MoveImagem(imagem, posi) {
            const dto = {
        id: imagem,   // precisa bater com o nome do DTO no C#
    posicao: posi
            };

    fetch("/Verify/MoveImagem", {
        method: "POST",
    headers: {"Content-Type": "application/json" },
    body: JSON.stringify(dto)
            })
                .then(res => {
                    if (!res.ok) {
                        throw new Error("Erro ao mover a imagem");
                    }
    return res.text(); // pode ser .json() se a API retornar JSON
                })
                .then(data => {
        console.log("Sucesso:", data);
                    // aqui você pode atualizar a UI (ex: recarregar tabela/lista)
                })
                .catch(err => {
        console.error("Falha:", err);
    alert("Erro ao mover a imagem");
                });
        }
    function updateSelection(index) {
            const tableBody = document.getElementById("tableBody");
    const rows = Array.from(tableBody.children);

    if (index < 0 || index >= rows.length) return;

            // Remove seleção antiga
            rows.forEach(r => r.classList.remove("selected"));

    // Seleciona a nova linha
    const newRow = rows[index];
    newRow.classList.add("selected");

    // Scroll automático
    newRow.scrollIntoView({behavior: "smooth", block: "nearest" });

    // Atualiza índice global
    selectedRowIndex = index;

    return newRow;
        }
    function soDigitos(e) {
            // bloqueia tudo que não for 0-9, Backspace, Delete, setas, Tab
            const k = e.key;
    if (!/[0-9]/.test(k) && !['Backspace', 'Delete', 'ArrowLeft', 'ArrowRight', 'Tab'].includes(k)) {
        e.preventDefault();
            }
        }
    function validaDoc() {
        const v = docInput.value.trim();
        if (v !== '') {

            console.log(v);
            ok = /^\d{10}$/.test(v);
            docErro.style.display = ok ? 'none' : 'block';
            btnSalvarDoc.disabled = !ok;
        } else {
            ok = true;
        }
        return ok;
            }
    function apply() {img.style.transform = `translate(${tx}px,${ty}px) scale(${scale})`; }
    const btnAddImg = document.getElementById('btnAddTif');
    const input = document.getElementById('fileTif');        
    btnAddImg.addEventListener('click', () => input.click());
    input.addEventListener('change', async () => {        
            const formData = new FormData();
    formData.append("idLote", idLote);
    formData.append("posicao", selectedRowIndex);

    Array.from(input.files || [])
                .filter(f => /\.tiff?$/i.test(f.name))
                .forEach(f => formData.append("files", f));


    const resp = await fetch('/Verify/InsereImagem', {method: 'POST', body: formData });
    if (!resp.ok) {alert('Falha no upload'); input.value = ''; return; }

    const files = formData.getAll("files");
                    files.forEach(file => {
                            const tr = document.createElement('tr');
    tr.className = 'table-row';
    const td1 = document.createElement('td'); // vazio
    const td2 = document.createElement('td'); td2.textContent = file.name;
    tr.append(td1, td2);

    tableBody.insertBefore(tr, tableBody.children[selectedRowIndex + 1]);
    updateSelection(selectedRowIndex+1);
                    });

    input.value = '';
        });
