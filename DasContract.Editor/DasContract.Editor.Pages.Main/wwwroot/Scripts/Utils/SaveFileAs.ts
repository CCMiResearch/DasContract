
//Source https://wellsb.com/csharp/aspnet/blazor-jsinterop-save-file/


(window as any).DasContractPages.SaveFile
    = function (filename: string, fileContent: string, contentType: string = "text/plain", charset: string = "utf-8")
    {
        //console.log(filename);
        //console.log(fileContent);
        //console.log(contentType);
        //console.log(charset);

        var link = document.createElement("a");
        link.download = filename;
        link.href = `data:${contentType};charset=${charset},` + encodeURIComponent(fileContent)
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }

