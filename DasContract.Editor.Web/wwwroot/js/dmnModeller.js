import DmnJs from 'dmn-js/lib/Modeler';

export async function createModeler(modelerXml) {
    if (window.dmnModeler != null) {
        window.dmnModeler.destroy();
    }

    window.dmnModeler = new DmnJs({
        keyboard: {
            bindTo: document
        },
    });

    if (modelerXml === '') {
        modelerXml = defaultDmn;
    }

    await window.dmnModeler.importXML(modelerXml);

    window.dmnModeler.attachTo('#dmnCanvas');
}

export async function saveXml() {
    let savedXml = (await window.dmnModeler.saveXML({format: false})).xml
    return savedXml;
}

let defaultDmn = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<definitions xmlns=\"https:\/\/www.omg.org\/spec\/DMN\/20191111\/MODEL\/\" id=\"definitions_0qcte86\" name=\"definitions\" namespace=\"http:\/\/camunda.org\/schema\/1.0\/dmn\" exporter=\"dmn-js (https:\/\/demo.bpmn.io\/dmn)\" exporterVersion=\"10.2.0\">\r\n  <decision id=\"decision_0gdyta1\" name=\"\">\r\n    <decisionTable id=\"decisionTable_1v6173a\">\r\n      <input id=\"input1\" label=\"\">\r\n        <inputExpression id=\"inputExpression1\" typeRef=\"string\">\r\n          <text><\/text>\r\n        <\/inputExpression>\r\n      <\/input>\r\n      <output id=\"output1\" label=\"\" name=\"\" typeRef=\"string\" \/>\r\n    <\/decisionTable>\r\n  <\/decision>\r\n<\/definitions>"