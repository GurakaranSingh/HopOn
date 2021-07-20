//var reader = {};
var Selectedfile = [];
var slice_size = 73400320;//70 mb 31457280;//30mb//
var MimimumSizeForChunk = 73400320//70 mb
var MaxChunkSerVer = 209715200//200 mb
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
//var next_slice = 0;
var _amazonunqID = "";
var Guid = "";
var FinalChunkSize = 0;
var ChucksCount = 0;
var MultiFileSelectArray = [];
var MultiFileSelectArrayFileName = [];

class FileUpload {

    constructor() {
    }


    start_upload(files, index) {
        //files = document.getElementById("fileToUpload").files;
        //var filelistcomponnent = document.getElementById('FileListcomponent')
        var ImageListComponent = document.getElementById('gallery_container')
        //  for (let i = 0; i < files.length; i++) {
        let Guid = new FileUpload().generateUUID() + "." + files["name"].split(".")[1];
        // getting file
        //Getting AWS UniqueID
        var xhr = new XMLHttpRequest();
        xhr.onload = function () {
            // Process our return data
            if (xhr.status == 200) {
                var result = JSON.parse(xhr.responseText);
                var fileModel = { File: files, AmazonID: result.uploadId, Guid: Guid, index: index, IsStop: false, IsNetworkFail: false }
                Selectedfile.push(fileModel);
                // time_start = new Date();
                //displaySpeed();
                var amazonID = "'" + result.uploadId + "'";
                if (files) {
                     
                    if ((files.type == 'image/png') || (files.type == 'image/jpg') || (files.type == 'image/jpeg')) {
                        var reader = new FileReader();
                        reader.onload = function (evt) {
                            var BodyImage = new FileUpload().GetProgressBarBody(result.uploadId, evt.target.result, amazonID, files.name,300)
                            ImageListComponent.insertAdjacentHTML("afterend",
                                BodyImage

                            );
                            //filelistcomponnent.insertAdjacentHTML("afterend",
                            //    );
                            //document.getElementById("image-container").innerHTML = imgTag;
                        };
                        reader.onerror = function (evt) {
                            console.error("An error ocurred reading the file", evt);
                        };
                        reader.readAsDataURL(files);

                    } else {
                        var BodyImage = new FileUpload().GetProgressBarBody(result.uploadId, "Images/mp4.png", amazonID, files.name, 160)
                        ImageListComponent.insertAdjacentHTML("afterend",
                            BodyImage

                        );
                    };
                }
            }
        }
        var obj = {
            fileSize: files.size,
            fileName: files.name.toString(),
            Guid: Guid,
        }
        xhr.open("POST", "api/Upload/GetUploadProject", false);
        xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
        xhr.send(JSON.stringify(obj));
        //}
    }

    GetProgressBarBody(uploadId, trgResult, amazonID, Filename, width) {
        var ProgressbarBody =
            '<div class="col-md-12" id="divcomponentID_' + uploadId + '">' +
            '<div class="thumbimg">' +
            '<img style="max-width:'+width+'px;" src="' + trgResult + '" alt="my image" />' +
            '<img src="images/cross_icn.svg" id="cancel_' + uploadId + '" onclick="CancelUploading(' + amazonID + ')" class="cross-icn">' +
            //'<div class="overlay">' +
            //'</div> ' +
            '</div>' +
            '<div class="col-md-12" id="' + uploadId + '">' +
            '<h3 class= "progress-title">' + Filename + '</h3>' +
            '<div class="progress">' +
            '<div class="progress-bar" id="progressbarid_' + uploadId + '" style="width:0%; background:#337ab7;">' +
            '<div class="progress-value" style="display:none" id="progressbarvalue_' + uploadId + '">0%' +
            '</div> ' +
            '</div>' +
            '</div>' +
            '<p class="progressBlock">' +
            // '<a href="#" id="start_' + result.uploadId + '"  style="display:inline-block" onclick="upload_file(' + 0 + ',' + amazonID + ',' + index + ')"><img src="Images/play-icon.svg" /></a>' +
            '<a href="#" style="display:inline-block" id="pause_' + uploadId + '" onclick="PauseUploading(' + amazonID + ')"> <img src="Images/pause-icon.svg" /></a>' +
            '<a href="#" id="resume_' + uploadId + '" style="display:none" onclick="Resume();">  <img src="Images/resume-icon.svg" /></a>' +
            '<a href="#" id="Retry_' + uploadId + '"  style="display:none" onclick="ReTryUploading(' + amazonID + ')"><img src="Images/retry-icon.svg" /></a>' +
            //'<button type="button" class="btn btn-danger" style="display:none" id="cancel_' + result.uploadId + '" onclick="CancelUploading(' + amazonID + ')">Cancel</button>' +
            //'<button type="button" id="Remove_' + result.uploadId + '" class="btn btn-danger" style="display:none" onclick="CancelUploading(' + amazonID + ')">Remove</button>' +
            '<span style="margin-left: 136px;" id="UploadTime_' + uploadId + '"></span>' +
            '</p>' +
            '</div>'
        '</div>';
        return ProgressbarBody;
    }
    generateUUID() { // Public Domain/MIT
        var d = new Date().getTime();//Timestamp
        var d2 = (performance && performance.now && (performance.now() * 1000)) || 0;//Time in microseconds since page-load or 0 if unsupported
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16;//random number between 0 and 16
            if (d > 0) {//Use timestamp until depleted
                r = (d + r) % 16 | 0;
                d = Math.floor(d / 16);
            } else {//Use microseconds since page-load if supported
                r = (d2 + r) % 16 | 0;
                d2 = Math.floor(d2 / 16);
            }
            return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
    }

    UploadInOneCall(file, amazonunqID, Guid, index) {
        var reader = {};
        reader = new FileReader();
        var xhr = new XMLHttpRequest();
        var blob = file.File;
        reader.onload = function (event) {
            xhr.onload = function () {
                if (xhr.status == 200) {
                    var result = JSON.parse(xhr.responseText);
                    var Progressvalueid = 'divcomponentID_' + amazonunqID;
                    document.getElementById(Progressvalueid).remove();
                    //setTimeout(function () {
                    Selectedfile = Selectedfile.filter(function (obj) {
                        return obj.AmazonID != amazonunqID
                    })
                    document.getElementById("FileListRefresh").click();
                }
            };
            var FileBase64Sting = event.target.result.toString();
            var obj = {
                FileName: file.File.name,
                //FilePath: "C:\\Users\\gurkaran.singh\\Downloads\\UploadUtilityHelper.cs",
                //awsUniqueId: amazonunqID,
                FileSize: file.File.size,
                ContentType: file.File.type,
                File: FileBase64Sting,
                awsUniqueId: amazonunqID,
                Guid: Guid
            }
            xhr.open("POST", "api/Upload/UploadInOneCall", true);
            xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
            xhr.send(JSON.stringify(obj));
             
            var Progressbaraid = 'progressbarid_' + amazonunqID;
            var Progressvalueid = 'progressbarvalue_' + amazonunqID;
            document.getElementById(Progressbaraid).style.width = 100 + '%';
            document.getElementById(Progressvalueid).innerHTML = 100 + '%';
            document.getElementById(Progressvalueid).innerHTML = 'Server is merging chunk please wait!.';

        };
        reader.readAsDataURL(blob);
    }

    upload_file(start, amazonunqID, index) {
        var reader = {};
        reader = new FileReader();
      //  new FileUpload().SHowHideButtonsOnStart(amazonunqID);
        let blob = "";
        _amazonunqID = amazonunqID;
        var file = Selectedfile.find(obj => {
            return obj.AmazonID === amazonunqID
        });
        if (file.File.size <= MimimumSizeForChunk) {
            new FileUpload().UploadInOneCall(file, amazonunqID, file.Guid, index);
            return;
        }
        else if (file.File.size >= MimimumSizeForChunk && file.File.size <= MaxChunkSerVer) {
            slice_size = 10485760;//10mb
        }
        else {
            slice_size = slice_size;
        }

        // ChucksCount = file.File.size / slice_size;
        //if (ChucksCount.toString().split('.')[1] > 0) { ChucksCount = + parseInt(ChucksCount.toString().split('.')[0]) + 1 }
        var next_slice = start + slice_size + 1;
        blob = file.File.slice(start, next_slice);
        if (next_slice > file.File.size) {
            next_slice = file.File.size;
            blob = file.File.slice(start);
        }
        reader.onload = function (event) {
            if (event.target.readyState !== FileReader.DONE) {
                return;
            }
            //Saving chunk file
            if (time_start == null) { time_start = new Date(); }
            var xhr = new XMLHttpRequest();
            if (!file.IsStop && !file.IsNetworkFail) {
                var aws = "'" + amazonunqID + "'"
                document.getElementById("resume_" + amazonunqID).setAttribute("onclick", "Resume(" + aws + "," + next_slice + "," + index + ")");
                xhr.onload = function () {
                    if (xhr.status == 200) {

                        if (xhr.response == "507") {
                            file.IsStop = true
                          //  new FileUpload().SHowHideButtonsOnStart(amazonunqID);
                            document.getElementById("pause_" + amazonunqID).style.display = "none";
                           // document.getElementById("cancel_" + amazonunqID).style.display = "none";
                            //document.getElementById("Remove_" + amazonunqID).style.display = "inline-block";
                            document.getElementById('progressbarid_' + amazonunqID).innerHTML = 'Insufficient Storage';
                            document.getElementById('progressbarid_' + amazonunqID).style.width = 100 + '%';
                            if (next_slice < file.File.size) {
                                // Update upload progress
                                // More to upload, call function recursively
                                chunkIndex = chunkIndex + 1;
                                new FileUpload().upload_file(next_slice, amazonunqID, index);
                            }
                        }
                        console.log(ChucksCount);
                        var result = JSON.parse(xhr.responseText);
                        //var Tags = { PartNumber: result["model"].partNumber, ETag: result["model"].eTag }
                        //prevetags.push(Tags);
                        end_time = new Date();
                        var size_done = start + slice_size;
                        var percent_done = Math.floor((size_done / file.File.size) * 100);

                        if (document.getElementById('progressbarvalue_' + amazonunqID) != null) {
                            if (document.getElementById('progressbarid_' + amazonunqID).innerHTML != "Insufficient Storage") {
                                var Progressbaraid = 'progressbarid_' + amazonunqID;
                                var Progressvalueid = 'progressbarvalue_' + amazonunqID;
                                console.log("ProgresbaarID = " + Progressbaraid + " Percentage = " + percent_done);
                                if (percent_done > 100) { percent_done = 100; }
                                //var UploadTime = 'UploadTime_' + file.File.name
                                if (!file.IsStop) {
                                    document.getElementById(Progressbaraid).style.width = percent_done + '%';
                                    document.getElementById(Progressvalueid).innerHTML = percent_done + '%';
                                }
                                if (document.getElementById(Progressvalueid).innerHTML != 'Insufficient Storage')
                                    if (percent_done == 100) { document.getElementById(Progressvalueid).innerHTML = 'Server is merging chunk please wait!.'; }
                            }
                        }
                        DataTransfer = DataTransfer + slice_size;
                        //document.getElementById(UploadTime).innerHTML = speedInMbps == undefined || speedInMbps < 0 ? 0 : speedInMbps + " Mbps";//displaySpeed(time_start, end_time, DataTransfer)
                        if (next_slice < file.File.size) {
                            // Update upload progress
                            // More to upload, call function recursively
                            chunkIndex = chunkIndex + 1;
                            new FileUpload().upload_file(next_slice, amazonunqID, index);
                        }
                        else {

                            // Update upload progress
                            //FInal Method call
                            document.getElementById(Progressvalueid).innerHTML = 'Server is merging chunk please wait!.';
                            new FileUpload().FinalCallMerging(amazonunqID, file, ChucksCount)
                        }
                    }
                    else {

                        new FileUpload().NetworkIssue(amazonunqID, file.File.name);
                    }
                };
                xhr.onerror = function () {

                    new FileUpload().NetworkIssue(amazonunqID, file.File.name);
                };

                var ChucknData = event.target.result.toString();
                var TestKey = "jsdkjsadkjaskjdkjsahdkjasdjkhsajkdsakjlhdkjsahdjsadij";
                var CLienHashKey = new SHA256Hash().SHA256(TestKey);
                var awsuniqueID = amazonunqID;
                var obj = {
                    chunkData: ChucknData,
                    awsUniqueId: awsuniqueID,
                    chunkMax: file.File.size,
                    chunkIndex: chunkIndex,
                    FileName: file.File.name,
                    Guid: file.Guid,
                    ClientHashKey: CLienHashKey
                }
                xhr.open("POST", "api/Upload/UploadingChunckBytes", true);
                xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
                xhr.send(JSON.stringify(obj));
            }
        };
        reader.readAsDataURL(blob);
    }
    checkValue(AwsId, guid, FileName) {
        document.getElementById(AwsId).addEventListener("change", function () {
            var cb = this;
            //console.log(cb["checked"],guid);
            if (cb["checked"] == true) {
                var isExist = MultiFileSelectArray.find(obj => {
                    return obj === guid
                });
                if (isExist != null) { return; }
                MultiFileSelectArray.push(guid);
                var FileObject = { FileName: FileName, Guid: guid };
                MultiFileSelectArrayFileName.push(FileObject);
            }
            else {
                MultiFileSelectArray = MultiFileSelectArray.filter(function (obj) {
                    return obj != guid
                })
                MultiFileSelectArrayFileName = MultiFileSelectArrayFileName.filter(function (obj) {
                    return obj.Guid != guid
                })
            }

            if (MultiFileSelectArray.length > 0) {
                document.getElementById("DownloadSelectedFile").style.display = "inline-block";
                document.getElementById("DeleteSelectedFile").style.display = "inline-block";
            }
            else {
                document.getElementById("DownloadSelectedFile").style.display = "none";
                document.getElementById("DeleteSelectedFile").style.display = "none";
            }
        });
    }

    DownloadDeleteMultiPel(Flag) {

        if (MultiFileSelectArray.length > 0) {
            if (Flag == "Download") {
                for (var i = 0; i < MultiFileSelectArray.length; i++) {
                    // DownloadFile(MultiFileSelectArray[i])
                    if (MultiFileSelectArray[i] != "undefined" && MultiFileSelectArray[i] != undefined)
                        // window.location = window.location.href + "api/Upload/Download/" + MultiFileSelectArray[i];
                        var FileId = MultiFileSelectArray[i];
                    var url = window.location.href + "api/Upload/Download/" + FileId
                    window.open(url, '_blank');
                    //document.getElementById(FileId).click();
                }
            }
            else if (Flag == "Delete") {
                var xhr = new XMLHttpRequest();
                xhr.onload = function () {
                    if (xhr.status == 200) {
                        document.getElementById("FileListRefresh").click();
                        document.getElementById("DownloadSelectedFile").style.display = "none";
                        document.getElementById("DeleteSelectedFile").style.display = "none";
                    }
                }
                var obj = {
                    Ids: MultiFileSelectArray,
                }
                xhr.open("POST", "api/Upload/DeleteMultipleFiles");
                xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
                xhr.send(JSON.stringify(obj));

            }
            else {
                console.log("Something went wrong");
            }
        }
        // document.getElementById("FileListRefresh").click();

    }
    FinalCallMerging(amazonunqID, file, ChucksCount) {
        var xhr = new XMLHttpRequest();
        xhr.onload = function () {
            if (xhr.status == 200) {

                if (xhr.response == "409") {
                    setTimeout(function () {
                        new FileUpload().FinalCallMerging(amazonunqID, file, ChucksCount)
                    }, 2000)
                }
                else if (xhr.response == "400") {
                    var Progressvalueid = 'progressbarvalue_' + amazonunqID;
                    document.getElementById(Progressvalueid).innerHTML = 'Something Happen wrong with file';
                    return;
                }
                else {
                    var Progressvalueid = 'progressbarvalue_' + amazonunqID;
                    document.getElementById(Progressvalueid).innerHTML = 'File Uploaded Succesfully';
                    //end_time = new Date();
                    // document.getElementById(UploadTime).innerHTML = speedInMbps == undefined || speedInMbps < 0 ? 0 : speedInMbps + " Mbps";//displaySpeed(time_start, end_time, DataTransfer)
                    var Progressvalueid = 'divcomponentID_' + amazonunqID;
                    document.getElementById(Progressvalueid).remove();
                    //setTimeout(function () {
                    Selectedfile = Selectedfile.filter(function (obj) {
                        return obj.AmazonID != amazonunqID
                    })
                    document.getElementById("FileListRefresh").click();
                    //}, 1000)
                    return;
                }
            }
        };
        var obj = {
            lastpart: true,
            UploadId: amazonunqID,
            prevETags: null,
            fileName: file.File.name,
            PartNumber: PartNumber,
            FileSize: file.File.size,
            Guid: file.Guid,
            ChucksCount: ChucksCount,
            FileType: file.File.type
        }
        xhr.open("POST", "api/Upload/FinalCallFOrCHunk");
        xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
        xhr.send(JSON.stringify(obj));
    }

    SHowHideButtonsOnStart(amazonunqID) {
        //document.getElementById("start_" + amazonunqID).style.display = "none";
        //document.getElementById("cancel_" + amazonunqID).style.display = "inline-block";
        //if (!IsStopTrue)
           // document.getElementById("pause_" + amazonunqID).style.display = "inline-block";
    }

    //function displaySpeed(time_start, end_time, LoadedBytes) {
    displaySpeed() {
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
    PauseUploading(amazonunqID) {
         
        //console.log("PauseCall");
        var file = Selectedfile.find(obj => {
            return obj.AmazonID === amazonunqID
        });
        file.IsStop = true;
        document.getElementById("pause_" + amazonunqID).style.display = "none";
        document.getElementById("resume_" + amazonunqID).style.display = "inline-block";
        //document.getElementById("resume_" + amazonunqID).setAttribute("onclick", "ResumeUploading('" + amazonunqID + "','" + next_slice + "')'");
    }
    NetworkIssue(amazonunqID, FileName) {
        var file = Selectedfile.find(obj => {
            return obj.AmazonID === amazonunqID
        });
        file.IsNetworkFail = true;
        document.getElementById("Retry_" + amazonunqID).style.display = "inline-block";
        document.getElementById("UploadTime_" + FileName).style.display = "none";
        document.getElementById("RetryMessage_" + amazonunqID).innerHTML = "Your Internet Network Fail! Please Try Again";
        document.getElementById("RetryMessage_" + amazonunqID).style.display = "inline-block";
        document.getElementById("start_" + amazonunqID).style.display = "none";
        document.getElementById("pause_" + amazonunqID).style.display = "none";
        document.getElementById("resume_" + amazonunqID).style.display = "none";
    }
    //ResumeUploading(next_slice,amazonunqID, index) {
    //    var file = Selectedfile.find(obj => {
    //        return obj.AmazonID === amazonunqID
    //    });
    //    file.IsStop = true;
    //    document.getElementById("pause_" + amazonunqID).style.display = "inline-block";
    //    document.getElementById("resume_" + amazonunqID).style.display = "none";
    //    upload_file(next_slice, _amazonunqID,index);
    //}
    ResumeFile(next_slice, _amazonunqID, _index) {
        upload_file(next_slice, _amazonunqID, _index);
    }
    ReTryUploading(amazonunqID) {
        var file = Selectedfile.find(obj => {
            return obj.AmazonID === amazonunqID
        });
        file.IsNetworkFail = true;
        document.getElementById("pause_" + amazonunqID).style.display = "inline-block";
        document.getElementById("resume_" + amazonunqID).style.display = "none";
        document.getElementById("Retry_" + amazonunqID).style.display = "none";
        document.getElementById("RetryMessage_" + amazonunqID).style.display = "none";
        upload_file(next_slice, _amazonunqID);
    }
    DeleteFile(FileName) {
        //Saving chunk file
        var xhr = new XMLHttpRequest();
        xhr.onload = function () {
            if (xhr.status == 200) {
                document.getElementById("FileListRefresh").click();
            };
        }
        xhr.open("POST", "api/Upload/DeleteAWSFile/" + FileName);
        xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
        xhr.send();
    }

    CancelUploading(awsid) {

        setTimeout(function () {
            var xhr = new XMLHttpRequest();
            xhr.onload = function () {
                if (xhr.status == 200) {
                    var Progressvalueid = 'divcomponentID_' + awsid;
                    document.getElementById(Progressvalueid).style.display = "none";
                };
            }
            xhr.open("POST", "api/Upload/CancleUploading?AWSID=" + awsid);
            xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
            xhr.send();
        }, 2000)
    }
}


