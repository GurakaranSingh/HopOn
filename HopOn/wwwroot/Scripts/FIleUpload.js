var reader = {};
var Selectedfile = [];
var slice_size = 21457280;
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


window.returnArrayAsync = () => {
    DotNet.invokeMethodAsync('HopOn', 'ReturnArrayAsync')
        .then(data => {
            //start_upload()
            files = document.getElementById("fileToUpload").files;
            var filelistcomponnent = document.getElementById('FileListcomponent')

            for (let i = 0; i < files.length; i++) {
                var Fileobject = { Size: files[i].size }
                //event.preventDefault();
                reader = new FileReader();
                // getting file
                //Getting AWS UniqueID
                var xhr = new XMLHttpRequest();
                xhr.onload = function () {
                    // Process our return data
                    if (xhr.status == 200) {
                        debugger
                        var result = JSON.parse(xhr.responseText);
                        var fileModel = { File: files[i], AmazonID: result.uploadId }
                        Selectedfile.push(fileModel);
                        localStorage.setItem("awsId", result.uploadId);
                        var file = Selectedfile
                        //upload_file(0);
                        time_start = new Date();
                        displaySpeed();
                        var amazonID = "'" + result.uploadId + "'";
                        //filelistcomponnent.insertAdjacentHTML("afterend",
                        //    //'<div class="col-md-12" id="divcomponentID_' + files[i].name + '"><h3 class="progress-title">' + files[i].name + ': </h3><div class="progress"><div class="progress-bar" id="progressbarid_' + files[i].name + '" style="width:0%; background:#97c513;"><div class="progress-value" id="progressbarvalue_' + files[i].name + '">0%</div></div></div><button type="button" class="btn btn-outline-success" onclick="upload_file(' + file + ',' + files[i].size + ')">Upload</button></div>');
                        //    '<div class="col-md-12" id="divcomponentID_' + files[i].name + '"><h3 class="progress-title">' + files[i].name + ': </h3><div class="progress"><div class="progress-bar" id="progressbarid_' + files[i].name + '" style="width:0%; background:#97c513;"><div class="progress-value" id="progressbarvalue_' + files[i].name + '">0%</div></div></div><p><button type="button" class="btn btn-outline-success" onclick="upload_file(' + 0 + ',' + amazonID + ')">Upload</button><span style="margin-left: 136px;" id="UploadTime_' + files[i].name + '"> </span></p></div>');
                        location.reload();
                    }
                };

                var obj = {
                    fileSize: files[i].size.toString(),
                    fileName: files[i].name.toString(),
                }
                xhr.open("POST", "api/Upload/GetUploadProject", false);
                xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
                xhr.send(JSON.stringify(obj));

                var file = "'" + files[i].name + "'";

            }
            var obj = {
                FileUploadModel: FileNamearray
            }
        });
};

function start_upload(name, size) {
    //event.preventDefault();
    reader = new FileReader();
    // getting file
    //Getting AWS UniqueID
    var xhr = new XMLHttpRequest();
    xhr.onload = function () {
        // Process our return data
        if (xhr.status == 200) {
            var result = JSON.parse(xhr.responseText);
            localStorage.setItem("awsId", result.uploadId);
            var file = Selectedfile

            upload_file(0);
        }
    };
    var obj = {
        fileSize: size.toString(),
        fileName: name.toString(),
    }
    xhr.open("POST", "api/Upload/GetUploadProject");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.send(JSON.stringify(obj));
}
function upload_file(start, amazonunqID) {
    debugger
    file = Selectedfile.find(obj => {
        return obj.AmazonID === amazonunqID
    });
    var next_slice = start + slice_size + 1;

    var blob = file.File.slice(start, next_slice);

    reader.onload = function (event) {
        if (event.target.readyState !== FileReader.DONE) {
            return;
        }
        //Saving chunk file
        if (time_start == null) { time_start = new Date();}
        var xhr = new XMLHttpRequest();


        xhr.onload = function () {
            if (xhr.status == 200) {
                var result = JSON.parse(xhr.responseText);
                var Tags = { PartNumber: result.partNumber, ETag: result.eTag }
                prevetags.push(Tags);
                end_time = new Date();
                var size_done = start + slice_size;
                var percent_done = Math.floor((size_done / file.File.size) * 100);
                var Progressbaraid = 'progressbarid_' + file.File.name;
                var Progressvalueid = 'progressbarvalue_' + file.File.name;
                if (percent_done > 100) { percent_done = 100; }
                var UploadTime = 'UploadTime_' + file.File.name
                document.getElementById(Progressbaraid).style.width = percent_done + '%';
                document.getElementById(Progressvalueid).innerHTML = percent_done + '%';
                DataTransfer = DataTransfer + slice_size;
                console.log(DataTransfer);
                document.getElementById(UploadTime).innerHTML = speedInMbps == undefined ? 0 : speedInMbps + " Mbps";//displaySpeed(time_start, end_time, DataTransfer)
                if (next_slice < file.File.size) {
                    // Update upload progress
                    // More to upload, call function recursively
                    chunkIndex = chunkIndex + 1;
                    upload_file(next_slice, amazonunqID);
                } else {
                    // Update upload progress
                    xhr.onload = function () {
                        var Progressvalueid = 'progressbarvalue_' + file.File.name;
                        document.getElementById(Progressvalueid).innerHTML = 'File Uploaded Succesfully';
                         end_time = new Date();
                        document.getElementById(UploadTime).innerHTML = speedInMbps == undefined ? 0 : speedInMbps + " Mbps";//displaySpeed(time_start, end_time, DataTransfer)
                        setTimeout(function () {
                            var Progressvalueid = 'divcomponentID_' + file.File.name;
                            document.getElementById(Progressvalueid).remove();
                        }, 3000)

                        setTimeout(function () { location.reload() }, 2000)
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
        xhr.open("POST", "api/Upload/UploadingChunckBytes", false);
        xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
        xhr.send(JSON.stringify(obj));
    };
    reader.readAsDataURL(blob);
}



//function displaySpeed(time_start, end_time, LoadedBytes) {
function displaySpeed() {
    setInterval(function () {
        if (DataTransfer > 0) {
            var timeDuration = (end_time - time_start) / 1000;

            var loadedBits = DataTransfer;

            /* Converts a number into string
               using toFixed(2) rounding to 2 */

            var bps = (loadedBits / timeDuration).toFixed(2);
            var speedInKbps = (bps / 1024).toFixed(2);
            speedInMbps = (speedInKbps / 1024).toFixed(2);
            console.log("Your internet connection speed is: \n"
                + bps + " bps\n" + speedInKbps
                + " kbps\n" + speedInMbps + " Mbps\n");
            DataTransfer = 0;
            time_start = null;
        }
        //return speedInMbps;
    }, 1000);

}