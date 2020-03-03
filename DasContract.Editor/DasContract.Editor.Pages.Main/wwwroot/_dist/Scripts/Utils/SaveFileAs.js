//Source https://wellsb.com/csharp/aspnet/blazor-jsinterop-save-file/
window.DasContractPages = {
    SaveFile: function (filename, fileContent, contentType, charset) {
        if (contentType === void 0) { contentType = "text/plain"; }
        if (charset === void 0) { charset = "utf-8"; }
        var link = document.createElement("a");
        link.download = filename;
        link.href = "data:" + contentType + ";charset=," + charset + encodeURIComponent(fileContent);
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }
};
