exports.__esModule = true;
var ajax_1 = require("@drozdik.m/ajax");
window.MaterialBootstrapRazorComponents.UploadableFileInput = {
    Upload: function (inputId, url, name, mediatorReference) {
        //console.log(`UploadableFileInput.Upload called { ${inputId} ${url} ${name} }`);
        //Find the input
        var fileInput = document.getElementById(inputId);
        if (fileInput == null) {
            console.error("(UploadableFileInput.Upload) Element #" + inputId + " not found");
            return;
        }
        //Get all files
        var files = fileInput.files;
        //console.log("Files:");
        //console.log(files);
        //START SENDING
        var currentId = 0;
        /**
         * Sends following file until there are any files
         * */
        var SendNextFile = function () {
            if (currentId >= files.length)
                return;
            //Setup ajax
            var ajax = new ajax_1.Ajax();
            ajax.OnSuccess.Add(function (caller, args) {
                mediatorReference.invokeMethodAsync("FinishFileUploading", currentId);
                //console.log(args.Response().Response());
            });
            ajax.OnError.Add(function (caller, args) {
                var error = "[" + args.Response().Status() + " - " + args.Response().StatusText() + "] " + args.Response().Response();
                mediatorReference.invokeMethodAsync("ErrorFileUploading", currentId, error);
                //console.log(error); 
            });
            ajax.OnSendProgress.Add(function (caller, args) {
                //console.log("Progress: " + args.Loaded());
                mediatorReference.invokeMethodAsync("UpdateFileStatus", currentId, args.Loaded());
            });
            ajax.OnFinish.Add(function (caller, args) {
                currentId++;
                SendNextFile();
            });
            //Trigger post
            //console.log("Uploading: " + currentId);
            var formData = new FormData();
            formData.append(name, files[currentId]);
            console.log(name);
            console.log(files[currentId]);
            ajax.Post(url, [], formData);
        };
        //Start sending
        SendNextFile();
    }
};
