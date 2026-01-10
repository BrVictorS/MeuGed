// Variáveis Globais
let selectedIndex = 0;
const selectElement = document.getElementById('docType');
const metadataPanel = document.getElementById('metadataPanel');
const tableBody = document.getElementById('tablebody');
const btnSalvarDoc = document.getElementById('salvaDoc');
const btnFinaliza = document.getElementById('btnFinaliza');
const previewImg = document.getElementById('previewImg');
let tipoDocSelecionado;

// Inicialização
document.addEventListener('DOMContentLoaded', () => {
    const rows = tableBody.children;
    if (rows.length > 0) {
        selecionarLinha(rows[0]);
    }
});

// --- LÓGICA DE SELEÇÃO E CARREGAMENTO ---

function selecionarLinha(row) {
    if (!row) return;

    // Remove seleção anterior
    Array.from(tableBody.children).forEach(r => r.classList.remove('selecionado'));

    // Aplica nova seleção
    row.classList.add('selecionado');
    selectedIndex = Array.from(tableBody.children).indexOf(row);

    // Scroll suave para manter a linha visível na lista
    row.scrollIntoView({ behavior: 'smooth', block: 'nearest' });

    limparCampos();
    carregarImagem(row);
    getIndexacaoDocumento(row);
}

// Carrega a imagem no painel direito
function carregarImagem(row) {
    // Pega apenas o texto limpo do documento (nome do arquivo/ID)
    const docNome = row.innerText.trim();

    // Feedback visual de loading (opcional)
    previewImg.style.opacity = '0.5';

    fetch("/Verify/MostrarImagemIndex", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(docNome)
    })
        .then(res => res.blob())
        .then(blob => {
            const url = URL.createObjectURL(blob);
            previewImg.src = url;
            previewImg.style.opacity = '1';
        })
        .catch(err => {
            console.error("Erro ao carregar imagem:", err);
            previewImg.style.opacity = '1';
        });
}

// Busca dados já indexados (se houver)
async function getIndexacaoDocumento(row) {
    const docNome = row.innerText.trim();

    try {
        const response = await fetch('/Index/GetIndexacaoDocumento/', {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(docNome)
        });

        if (!response.ok) return; // Se não tiver dados, apenas segue

        const dadosJson = await response.json();

        // Seleciona o Tipo Documental no Dropdown
        if (dadosJson['idTipoDoc']) {
            selectElement.value = dadosJson['idTipoDoc'];
            // Dispara o evento de mudança manualmente para recriar os campos
            selectElement.dispatchEvent(new Event('change'));

            // Preenche os metadados
            // Pequeno delay para garantir que o DOM dos inputs foi criado
            setTimeout(() => {
                const metadadosInputs = Array.from(metadataPanel.querySelectorAll('input'));

                if (dadosJson.metadados) {
                    dadosJson.metadados.forEach(metadado => {
                        const input = metadadosInputs.find(x => x.id == metadado['id']);
                        if (input) input.value = metadado['valor'];
                    });
                }
            }, 50);
        }

    } catch (error) {
        console.error("Erro ao buscar indexação:", error);
    }
}

// --- GERENCIAMENTO DE METADADOS ---

// Ouve mudança no Dropdown de Tipos
selectElement.addEventListener('change', function (event) {
    tipoDocSelecionado = event.target.value;
    construirFormularioMetadados(tipoDocSelecionado);
});

function construirFormularioMetadados(idDoTipo) {
    metadataPanel.innerHTML = '';

    if (!idDoTipo) return;

    // 'tiposDocumento' vem da View (Global variable)
    const tipoObj = tiposDocumento.find(t => t.id == parseInt(idDoTipo));

    if (tipoObj && tipoObj.metadados) {
        tipoObj.metadados.forEach((meta, index) => {
            // Container
            const div = document.createElement('div');
            div.className = 'mb-3'; // Espaçamento

            // Label
            const label = document.createElement('label');
            label.className = 'block text-xs font-bold text-gray-700 uppercase mb-1';
            label.textContent = meta.nome;
            label.htmlFor = meta.id;

            // Input
            const input = document.createElement('input');
            input.type = 'text';
            input.id = meta.id;
            input.name = meta.nome;
            input.className = 'form-control'; // Classe definida no style da View para compatibilidade
            input.autocomplete = "off";

            // Foca no primeiro campo automaticamente
            if (index === 0) {
                setTimeout(() => input.focus(), 100);
            }

            // Tecla Enter pula para o próximo campo (comportamento de indexação rápida)
            input.addEventListener('keydown', (e) => {
                if (e.key === 'Enter') {
                    e.preventDefault();
                    const nextInput = metadataPanel.querySelectorAll('input')[index + 1];
                    if (nextInput) {
                        nextInput.focus();
                    } else {
                        // Se for o último campo, tenta salvar
                        btnSalvarDoc.click();
                    }
                }
            });

            div.appendChild(label);
            div.appendChild(input);
            metadataPanel.appendChild(div);
        });
    }
}

function limparCampos() {
    metadataPanel.innerHTML = '<div class="text-center py-8 text-gray-400 italic text-xs">Selecione um tipo documental acima.</div>';
    selectElement.value = "Select a type"; // Reseta select (ajuste conforme o value do option default)
    selectElement.selectedIndex = 0;
}


// --- INTERAÇÃO COM TABELA ---

// Clique na linha
tableBody.addEventListener('click', function (e) {
    const row = e.target.closest('tr');
    if (row) {
        selecionarLinha(row);
    }
});

// Navegação por Teclado (Setas) na Tabela
document.addEventListener('keydown', function (e) {
    // Ignora se estiver digitando em um input (exceto F10)
    if (e.target.tagName === 'INPUT' && e.key !== 'F10') return;

    const rows = Array.from(tableBody.children);

    if (e.key === 'ArrowDown') {
        e.preventDefault();
        if (selectedIndex < rows.length - 1) {
            selecionarLinha(rows[selectedIndex + 1]);
        }
    } else if (e.key === 'ArrowUp') {
        e.preventDefault();
        if (selectedIndex > 0) {
            selecionarLinha(rows[selectedIndex - 1]);
        }
    }
});


// --- SALVAR ---

// Atalho F10
document.addEventListener('keydown', function (e) {
    if (e.key === 'F10') {
        e.preventDefault();
        e.stopPropagation();
        btnSalvarDoc.click();
    }
});

btnSalvarDoc.addEventListener('click', async function (e) {
    // 1. Validação
    const docRow = document.querySelector('.selecionado');
    if (!docRow) {
        if (typeof showMensagem === "function") showMensagem('Selecione um documento!', true);
        else alert('Selecione um documento!');
        return;
    }

    if (selectElement.selectedIndex === 0) {
        if (typeof showMensagem === "function") showMensagem('Selecione o Tipo Documental!', true);
        else alert('Selecione o Tipo Documental!');
        selectElement.focus();
        return;
    }

    // 2. Coleta de Dados
    const inputs = metadataPanel.querySelectorAll('input');
    const docNome = docRow.innerText.trim();

    const payload = {
        idDocumento: docNome,
        idTipoDoc: selectElement.value,
        idLote: idLote, // Variável global injetada na View
        metadados: []
    };

    inputs.forEach(input => {
        payload.metadados.push({
            id: input.id,
            valor: input.value
        });
    });

    // 3. Envio
    try {
        // Feedback visual no botão
        const btnOriginalText = btnSalvarDoc.innerHTML;
        btnSalvarDoc.innerHTML = '<div class="animate-spin inline-block w-4 h-4 border-2 border-white border-t-transparent rounded-full mr-2"></div> Salvando...';
        btnSalvarDoc.disabled = true;

        const response = await fetch('/Index/InsereIndexacaoDocumento', {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });

        const htmlResult = await response.text();

        // Verifica se showMensagem existe (do seu layout padrão)
        if (typeof showMensagem === "function") {
            showMensagem(htmlResult); // Assume que o backend retorna msg HTML ou texto puro
        } else {
            console.log("Salvo:", htmlResult);
        }

        // 4. Auto-Avanço (Workflow de produção)
        autoAvancar();

    } catch (err) {
        console.error(err);
        alert("Erro ao salvar documento.");
    } finally {
        // Restaura botão
        btnSalvarDoc.innerHTML = '<i class="bi bi-save"></i> Salvar (F10)';
        btnSalvarDoc.disabled = false;
    }
});

function autoAvancar() {
    const rows = Array.from(tableBody.children);

    // Marca visualmente que o anterior foi salvo (opcional - verde fraco)
    if (rows[selectedIndex]) {
        rows[selectedIndex].classList.add('bg-green-50');
        // Adiciona ícone de check se não tiver
        const cell = rows[selectedIndex].querySelector('td');
        if (!cell.querySelector('.bi-check')) {
            cell.innerHTML += ' <i class="bi bi-check text-green-600 ml-auto"></i>';
        }
    }

    // Move para o próximo
    if (selectedIndex < rows.length - 1) {
        selecionarLinha(rows[selectedIndex + 1]);
    } else {
        if (typeof showMensagem === "function") showMensagem("Último documento do lote indexado!");
    }
}

// Botão Finalizar (Placeholder)
btnFinaliza.addEventListener('click', (e) => {
    // A ação principal já está no `asp-action` do link envolvendo o botão, 
    // mas se precisar de validação extra via JS, faça aqui.
    if (!confirm("Deseja realmente finalizar o lote?")) {
        e.preventDefault();
    }
});