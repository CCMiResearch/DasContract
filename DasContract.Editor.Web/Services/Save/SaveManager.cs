﻿using DasContract.Editor.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.Save
{
    /// <summary>
    /// Certain parts of the modeler are not saved after every user input (for example the DMN editor runs entirely in javascript and is only
    /// saved into the .net data model when explicitly requested).
    /// To make sure that all progress is saved, modules that do not save their state after every user input should subscribe to the SaveRequested event
    /// When a certain action requires all progress to be saved (exiting a modeler tab/switching to a different modeler window), it can call the 
    /// RequestSave method, which notifies all of the subscribers that they should save their progress. This method is awaitable, as most of the save
    /// operations are asynchronous!
    /// </summary>
    public class SaveManager
    {
        public AsyncEvent<EventArgs> StateSaveRequested = new AsyncEvent<EventArgs>();
        public AsyncEvent<EventArgs> ContractSaveRequested = new AsyncEvent<EventArgs>();

        public async Task RequestStateSave()
        {
            await StateSaveRequested.InvokeAsync(this, EventArgs.Empty);

        }

        public async Task RequestContractSave()
        {
            await RequestStateSave();
            await ContractSaveRequested.InvokeAsync(this, EventArgs.Empty);
        }
    }
}
