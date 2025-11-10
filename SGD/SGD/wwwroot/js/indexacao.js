
let selectedIndex;
const selectElement = document.getElementById('docType');
const metadataPanel = document.getElementById('metadataPanel');
const tablebody = document.getElementById('tablebody');
const btnSalvarDoc = document.getElementById('salvaDoc');
const btnFinaliza = document.getElementById('btnFinaliza');
let tipodocselecionado;

document.addEventListener('DOMContentLoaded', () => {
    tablebody.children[0].click();

});

async function getIndexacaoDocumento() {
    const doc = document.querySelector('.selecionado');

    var response = await fetch('/Index/GetIndexacaoDocumento/', {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(doc.outerText)

    });

    const dadosJson = await response.json();
    selectElement[Array.from(selectElement.options).find(x => x.value == parseInt(dadosJson['idTipoDoc'])).index].selected = true;
    carregaMetadados(parseInt(dadosJson['idTipoDoc']));

    const metadadosCarregados = Array.from(metadataPanel.querySelectorAll('input'));

    dadosJson.metadados.forEach((metadado, x) => {
        metadadosCarregados.find(x => x.id == parseInt(metadado['id'])).value = metadado['valor'];
    });

    // Se você realmente precisar usar alert, converta o objeto em string
    //alert("Dados recebidos: \n" + JSON.stringify(dadosJson, null, 2));
}

function limparCampos() {
    metadataPanel.querySelectorAll('input').forEach((campo, x) => {
        campo.value = '';
    });
}
function carregaMetadados(idDoTipo) {
    metadataPanel.innerHTML = '';

    if (!idDoTipo) {
        return;
    }

    const tipoSelecionado = tiposDocumento.find(tipo => tipo.id == parseInt(idDoTipo));

    if (tipoSelecionado && tipoSelecionado.metadados) {
        tipoSelecionado.metadados.forEach(meta => {
            const formGroup = document.createElement('div');
            formGroup.className = 'mb-3';

            const label = document.createElement('label');
            label.className = 'form-label';
            label.textContent = meta.nome;
            label.htmlFor = 'meta_' + meta.nome;

            const input = document.createElement('input');
            input.className = 'form-control';
            input.type = 'text';
            input.id = meta.id;
            input.name = meta.nome;

            formGroup.appendChild(label);
            formGroup.appendChild(input);
            metadataPanel.appendChild(formGroup);
        });
    }
}

selectElement.addEventListener('change', function (event) {
    tipodocselecionado = event.target.value;
    carregaMetadados(tipodocselecionado);
});

tablebody.addEventListener('click', function (e) {
    const row = e.target.closest('tr');
    tablebody.querySelectorAll('tr').forEach(r => r.classList.remove('selecionado'));
    selectedIndex = Array.from(tablebody.children).indexOf(row);
    row.classList.add('selecionado');
    limparCampos();
    getIndexacaoDocumento();

    const previewImg = document.getElementById('previewImg');

    fetch("/Verify/MostrarImagemIndex", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(row.outerText)
    })
        .then(res => res.blob())
        .then(blob => {
            previewImg.src = URL.createObjectURL(blob);
        });


});

btnSalvarDoc.addEventListener('click', async function (e) {
    const metadados = document.getElementById('metadataPanel').querySelectorAll('input');
    const doc = document.querySelector('.selecionado');

    if (doc) {
        const valores = {};
        valores['idDocumento'] = doc.outerText;
        valores['idTipoDoc'] = selectElement.value;
        valores['idLote'] = idLote;
        valores['metadados'] = [];

        metadados.forEach(input => {
            valores['metadados'].push({
                id: input.id,
                valor: input.value
            });
        });

        await fetch('/Index/InsereIndexacaoDocumento', {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(valores)
        })
            .then(r => r.text())
            .then(html => showMensagem(html));

    } else {
        showMensagem('Selecione um documento para ser indexado!', false);
    }
});

document.addEventListener('keydown', function (e) {
    if (e.key === 'F10') {
        e.preventDefault();   // bloqueia o menu de contexto do navegador
        e.stopPropagation();  // impede propagação (opcional)
        btnSalvarDoc.click();
        
    }
});

btnFinaliza.addEventListener('click', (e) => {
    
});