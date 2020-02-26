import { Ajax } from "@drozdik.m/ajax";

(window as any).MaterialBootstrapRazorComponents.UploadableFileInput = {

    Upload: function (inputId: string, url: string, name: string, mediatorReference: any)
    {
        //console.log(`UploadableFileInput.Upload called { ${inputId} ${url} ${name} }`);
        
        //Find the input
        let fileInput = document.getElementById(inputId) as HTMLElement;
        if (fileInput == null)
        {
            console.error(`(UploadableFileInput.Upload) Element #${inputId} not found`);
            return;
        }

        //Get all files
        let files = (fileInput as any).files as FileList;
        //console.log("Files:");
        //console.log(files);

        //START SENDING
        let currentId = 0;

        /**
         * Sends following file until there are any files
         * */
        let SendNextFile = function()
        {
            if (currentId >= files.length)
                return;

            //Setup ajax
            let ajax = new Ajax();
            ajax.OnSuccess.Add(function (caller, args)
            {
                (mediatorReference as any).invokeMethodAsync("FinishFileUploading", currentId);
                //console.log(args.Response().Response());
            });
            ajax.OnError.Add(function (caller, args)
            {
                let error = `[${args.Response().Status()} - ${args.Response().StatusText()}] ${args.Response().Response()}`;
                (mediatorReference as any).invokeMethodAsync("ErrorFileUploading", currentId, error);
                //console.log(error); 
            });
            ajax.OnSendProgress.Add(function (caller, args)
            {
                //console.log("Progress: " + args.Loaded());
                (mediatorReference as any).invokeMethodAsync("UpdateFileStatus", currentId, args.Loaded());  
            });
            ajax.OnFinish.Add(function (caller, args)
            {
                currentId++;
                SendNextFile();
            });

            //Trigger post
            //console.log("Uploading: " + currentId);
            let formData = new FormData(); 
            formData.append(name, files[currentId]);
            console.log(name);
            console.log(files[currentId]);
            ajax.Post(url, [], formData);
        }

        //Start sending
        SendNextFile();
    }

}