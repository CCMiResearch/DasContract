import { saveAs } from 'file-saver';

export function saveFile(fileName, fileContent) {
    var FileSaver = require('file-saver');
    var blob = new Blob([fileContent], { type: "text/plain;charset=utf-8" });
    FileSaver.saveAs(blob, fileName);
}