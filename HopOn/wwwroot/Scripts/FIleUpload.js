var reader = {};
var Selectedfile = [];
var slice_size = 73400320;//80 mb
var chunkIndex = 0;
//$("#upload").on('click', start_upload);
var prevetags = [];
var PartNumber = 0;
var FileNamearray = [];
var DataTransfer = 0;
var t;
var time_start = new Date();
var end_time = new Date();
var time;
var speedInMbps;
var IsStopTrue = false;
var IsNetworFail = false;
var next_slice = 0;
var _amazonunqID = "";


window.returnArrayAsync = () => {
    DotNet.invokeMethodAsync('HopOn', 'ReturnArrayAsync')
        .then(data => {
            start_upload()
            document.getElementById('fileToUpload').value = null
        });
};
function GetProgressList() {
    debugger
    var xhr = new XMLHttpRequest();
    xhr.onload = function () {
        // Process our return data
        if (xhr.status == 200) {
            var result = JSON.parse(xhr.responseText);
            if (Selectedfile.length == 0) {
                for (var i = 0; i < result.length; i++) {
                    var Remove = 'Remove_' + result[i]["awsId"];
                    var start = 'start_' + result[i]["awsId"];
                    var pause = 'pause_' + result[i]["awsId"];
                    var resume = 'resume_' + result[i]["awsId"];
                    var cancel = 'cancel_' + result[i]["awsId"];
                    var Retry = 'Retry_' + result[i]["awsId"];
                    document.getElementById(Remove).style.display = "inline-block";
                    document.getElementById(start).style.display = "none";
                    document.getElementById(pause).style.display = "none";
                    document.getElementById(resume).style.display = "none";
                    document.getElementById(cancel).style.display = "none";
                    document.getElementById(Retry).style.display = "none";
                }
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    var Remove = 'Remove_' + result[i]["awsId"];
                    var start = 'start_' + result[i]["awsId"];
                    var pause = 'pause_' + result[i]["awsId"];
                    var resume = 'resume_' + result[i]["awsId"];
                    var cancel = 'cancel_' + result[i]["awsId"];
                    var Retry = 'Retry_' + result[i]["awsId"];
                    var fileexist = Selectedfile.find(obj => {
                        return obj.AmazonID === result[i]["awsId"]
                    });
                    if (fileexist == null || fileexist == undefined || fileexist == "null") {
                        document.getElementById(Remove).style.display = "inline-block";
                        document.getElementById(start).style.display = "none";
                        document.getElementById(pause).style.display = "none";
                        document.getElementById(resume).style.display = "none";
                        document.getElementById(cancel).style.display = "none";
                        document.getElementById(Retry).style.display = "none";
                    }
                }
            }
        }
    };
    xhr.open("GET", "api/Upload/GetAllProgressFile", false);
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.send();
}

window.onload = function afterPageLoad() {
    setTimeout(function () {
        GetProgressList();
    }, 100)
}

function start_upload() {
    files = document.getElementById("fileToUpload").files;
    for (let i = 0; i < files.length; i++) {
        reader = new FileReader();
        // getting file
        //Getting AWS UniqueID
        var xhr = new XMLHttpRequest();
        xhr.onload = function () {

            // Process our return data
            if (xhr.status == 200) {
                var result = JSON.parse(xhr.responseText);
                var fileModel = { File: files[i], AmazonID: result.uploadId }
                Selectedfile.push(fileModel);
                localStorage.setItem("awsId", result.uploadId);

               // time_start = new Date();
                //displaySpeed();
                setTimeout(function () { 
                    document.getElementById("ProgressbarRefresh").click();
                }, 1000)
            }
        };
        var obj = {
            fileSize: files[i].size.toString(),
            fileName: files[i].name.toString(),
        }
        xhr.open("POST", "api/Upload/GetUploadProject", false);
        xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
        xhr.send(JSON.stringify(obj));
    }
}


function upload_file(start, amazonunqID) {
    SHowHideButtonsOnStart(amazonunqID);
    _amazonunqID = amazonunqID;
    file = Selectedfile.find(obj => {
        return obj.AmazonID === amazonunqID
    });
    next_slice = start + slice_size + 1;

    var blob = file.File.slice(start, next_slice);

    reader.onload = function (event) {
        if (event.target.readyState !== FileReader.DONE) {
            return;
        }
        //Saving chunk file
        if (time_start == null) { time_start = new Date(); }
        var xhr = new XMLHttpRequest();


        if (!IsStopTrue && !IsNetworFail) {
            xhr.onload = function () {
                if (xhr.status == 200) {
                    var result = JSON.parse(xhr.responseText);
                    var Tags = { PartNumber: result["model"].partNumber, ETag: result["model"].eTag }
                    prevetags.push(Tags);
                    end_time = new Date();
                    var size_done = start + slice_size;
                    var percent_done = Math.floor((size_done / file.File.size) * 100);
                    var Progressbaraid = 'progressbarid_' + amazonunqID;
                    var Progressvalueid = 'progressbarvalue_' + amazonunqID;
                    if (percent_done > 100) { percent_done = 100; }
                    var UploadTime = 'UploadTime_' + file.File.name
                    if (!IsStopTrue) {
                        document.getElementById(Progressbaraid).style.width = percent_done + '%';
                        document.getElementById(Progressvalueid).innerHTML = percent_done + '%';
                    }
                    if (percent_done == 100) { document.getElementById(Progressvalueid).innerHTML = 'Server is merging chunk please wait!.'; }
                    DataTransfer = DataTransfer + slice_size;
                    //document.getElementById(UploadTime).innerHTML = speedInMbps == undefined || speedInMbps < 0 ? 0 : speedInMbps + " Mbps";//displaySpeed(time_start, end_time, DataTransfer)
                    if (next_slice < file.File.size) {
                        // Update upload progress
                        // More to upload, call function recursively
                        chunkIndex = chunkIndex + 1;
                        upload_file(next_slice, amazonunqID);
                    } else {
                        // Update upload progress
                        xhr.onload = function () {
                            var Progressvalueid = 'progressbarvalue_' + amazonunqID;
                            document.getElementById(Progressvalueid).innerHTML = 'File Uploaded Succesfully';
                            //end_time = new Date();
                           // document.getElementById(UploadTime).innerHTML = speedInMbps == undefined || speedInMbps < 0 ? 0 : speedInMbps + " Mbps";//displaySpeed(time_start, end_time, DataTransfer)
                                var Progressvalueid = 'divcomponentID_' + amazonunqID;
                                document.getElementById(Progressvalueid).remove();
                            debugger
                            //setTimeout(function () {
                            Selectedfile = Selectedfile.filter(function (obj) {
                                return obj.AmazonID != amazonunqID
                            })
                                document.getElementById("FileListRefresh").click();
                            //}, 1000)
                            return;
                        };
                        var obj = {
                            lastpart: true,
                            UploadId: amazonunqID,
                            prevETags: prevetags,
                            fileName: file.File.name,
                            PartNumber: PartNumber,
                            FileSize: file.File.size.toString()
                        }
                        xhr.open("POST", "api/Upload/FinalCallFOrCHunk");
                        xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
                        xhr.send(JSON.stringify(obj));
                    }
                }
                else {

                    NetworkIssue(amazonunqID, file.File.name);
                }
            };
            xhr.onerror = function () {

                NetworkIssue(amazonunqID, file.File.name);
            };

            var ChucknData = event.target.result.toString();
            var awsuniqueID = amazonunqID;
            var obj = {
                chunkData: ChucknData,
                awsUniqueId: awsuniqueID,
                chunkMax: file.File.size,
                chunkIndex: chunkIndex,
                FileName: file.File.name,
            }
            xhr.open("POST", "api/Upload/UploadingChunckBytes", true);
            xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
            xhr.send(JSON.stringify(obj));
        }
    };
    reader.readAsDataURL(blob);
}

function SHowHideButtonsOnStart(amazonunqID) {
    document.getElementById("start_" + amazonunqID).style.display = "none";
    document.getElementById("cancel_" + amazonunqID).style.display = "inline-block";

    if (!IsStopTrue)
        document.getElementById("pause_" + amazonunqID).style.display = "inline-block";
}

//function displaySpeed(time_start, end_time, LoadedBytes) {
function displaySpeed() {
    setInterval(function () {
        if (DataTransfer > 0) {
            console.log("ENd Time = " + end_time + "  " + " Start Time = " + time_start);
            var timeDuration = (end_time - time_start) / 1000;
            var loadedBits = DataTransfer;

            /* Converts a number into string
               using toFixed(2) rounding to 2 */
            var bps = (loadedBits / timeDuration).toFixed(2);
            var speedInKbps = (bps / 1024).toFixed(2);
            speedInMbps = (speedInKbps / 1024).toFixed(2);

            console.log("bps= " + bps);
            console.log("loadedBits= " + loadedBits);
            console.log("timeDuration= " + timeDuration);

            //console.log("Your internet connection speed is: \n"
            //    + bps + " bps\n" + speedInKbps
            //    + " kbps\n" + speedInMbps + " Mbps\n");
            DataTransfer = 0;
            time_start = null;
        }
        //return speedInMbps;
    }, 1000);
}
function PauseUploading(amazonunqID) {
    console.log("PauseCall");
    IsStopTrue = true;
    document.getElementById("pause_" + amazonunqID).style.display = "none";
    document.getElementById("resume_" + amazonunqID).style.display = "inline-block";
}
function NetworkIssue(amazonunqID, FileName) {

    IsNetworFail = true;
    document.getElementById("Retry_" + amazonunqID).style.display = "inline-block";
    document.getElementById("UploadTime_" + FileName).style.display = "none";
    document.getElementById("RetryMessage_" + amazonunqID).innerHTML = "Your Internet Network Fail! Please Try Again";
    document.getElementById("RetryMessage_" + amazonunqID).style.display = "inline-block";
    document.getElementById("start_" + amazonunqID).style.display = "none";
    document.getElementById("pause_" + amazonunqID).style.display = "none";
    document.getElementById("resume_" + amazonunqID).style.display = "none";
}
function ResumeUploading(amazonunqID) {

    IsStopTrue = false
    document.getElementById("pause_" + amazonunqID).style.display = "inline-block";
    document.getElementById("resume_" + amazonunqID).style.display = "none";
    upload_file(next_slice, _amazonunqID);
}
function ReTryUploading(amazonunqID) {
    IsNetworFail = false
    document.getElementById("pause_" + amazonunqID).style.display = "inline-block";
    document.getElementById("resume_" + amazonunqID).style.display = "none";
    document.getElementById("Retry_" + amazonunqID).style.display = "none";
    document.getElementById("RetryMessage_" + amazonunqID).style.display = "none";
    upload_file(next_slice, _amazonunqID);
}
function DeleteFile(FileName) {

    //Saving chunk file
    var xhr = new XMLHttpRequest();
    xhr.onload = function () {
        if (xhr.status == 200) {
            document.getElementById("FileListRefresh").click();
        };
    }
    xhr.open("POST", "api/Upload/DeleteAWSFile?FileName=" + FileName);
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.send();
}
function DownloadFile(FileName) {

    //Saving chunk file
    var xhr = new XMLHttpRequest();
    xhr.onload = function () {
        if (xhr.status == 200) {
        };
    }
    xhr.open("POST", "api/Upload/DownloadAWSFile?FileName=" + FileName);
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.send();
}

function CancelUploading(awsid) {
    IsStopTrue = true;
    setTimeout(function () {
        var xhr = new XMLHttpRequest();
        xhr.onload = function () {
            if (xhr.status == 200) {
                document.getElementById("ProgressbarRefresh").click();
            };
        }
        xhr.open("POST", "api/Upload/CancleUploading?AWSID=" + awsid);
        xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
        xhr.send();
    }, 2000)
}
