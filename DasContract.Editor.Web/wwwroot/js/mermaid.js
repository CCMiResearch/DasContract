import mermaid from 'mermaid/dist/mermaid.min.js';
import { downloadSvg, downloadSvgAsPng } from '../js/fileSaver';

const getSvgString = () => {
	const svg = $('#graph')[0];
	debugger;
	const svgString = svg.outerHTML
		.replaceAll('<br>', '<br/>')
		.replaceAll(/<img([^>]*)>/g, (m, g) => `<img ${g} />`);
	return svgString;
};


export const downloadSVG = (contractName) => {
	downloadSvg(
		`data-model-${contractName}.svg`,
		getSvgString()
	);
}

export const downloadPNG = (contractName) => {
	const svgElement = $('#graph')[0];
	const box = svgElement.getBoundingClientRect();
	downloadSvgAsPng(
		`data-model-${contractName}.png`,
		getSvgString(),
		box.width,
		box.height
	);
}

export function renderMermaidDiagram(elementId, input) {
	var canvas = document.getElementById(elementId);
	try {
		mermaid.mermaidAPI.render('graph', input, function (svgCode) {
			console.log("hello");
			canvas.innerHTML = svgCode;
			let svgElement = $('#graph')[0];
			let minWidth = window.getComputedStyle(svgElement).getPropertyValue('max-width');
			svgElement.style.setProperty('min-width', minWidth);
		})
	} catch (e) {
		let dgraph = $('#graph')[0];
		dgraph.remove();
    }
}