window.returnArrayAsync = () => {
    DotNet.invokeMethodAsync('HopOn', 'ReturnArrayAsync')
        .then(data => {
            var files = document.getElementById("fileToUpload").files;
            debugger
            var FileUploadClass = [];
            for (let i = 0; i < files.length; i++) {
                var FileStartObj = { AwsId: files[i] }
                FileUploadClass[i] = new FileUpload();
                FileUploadClass[i].start_upload(files[i], Math.random());
            }
            document.getElementById('AllDownloadButton').style.display = "inline-block";
            document.getElementById('fileToUpload').value = null
        });
};
function upload_file(start, amazonunqID, index) {
    var uploadClass = [];
    uploadClass[index] = new FileUpload();
    uploadClass[index].upload_file(start, amazonunqID,index);
}
function StartAllDownload() {
    var uploadClass = [];
    for (var i = 0; i < Selectedfile.length; i++) {
        uploadClass[i] = new FileUpload();
        uploadClass[i].upload_file(0, Selectedfile[i]["AmazonID"], Selectedfile[i]["index"])
    }
}
function DeleteFile(FileName) {
    let uploadClass = new FileUpload();
    uploadClass.DeleteFile(FileName);
}
function DownloadDeleteMultiPel(Flag) {
    debugger
    let uploadClass = new FileUpload();
    if (Flag == "Delete") {
        var filelistcomponnent = document.getElementById('FirstHead')
        for (var i = 0; i < MultiFileSelectArrayFileName.length; i++) {
            var Guid = "'" + "Table_" + MultiFileSelectArrayFileName[i].Guid + "'";
            filelistcomponnent.insertAdjacentHTML("afterend",
                '<tr id = ' + Guid + '>' +
                '<td style="border: 1px solid #ddd;padding: 8px;"> ' + MultiFileSelectArrayFileName[i].FileName + '</td >' +
                '<td>' +
                '<button type = "button" class= "btn btn-primary" onclick="RemoveItemFromList(' + Guid + ')"> Remove Item</button >' +
                '</td>' +
                '</tr>');
        }
        document.getElementById('DeleteConfirmModal').style.display = "inline-block";
    }
    else {
        document.getElementById('DeleteConfirmModal').style.display = "none";
        uploadClass.DownloadDeleteMultiPel(Flag);
    }
}
function ShareFile(Guid) {
    debugger
    var url = window.location.href + "api/Upload/DownloadAWSFile/" + Guid
    document.getElementById("ShareLink").value = url
    document.getElementById('ShareFileLink').style.display = "inline-block";
}
function RemoveItemFromList(Guid) {
    debugger
    var guid = Guid;
    var row = document.getElementById(guid);
    row.parentNode.removeChild(row);
    //document.getElementById(Guid).remove();
    MultiFileSelectArrayFileName = MultiFileSelectArrayFileName.filter(function (obj) {
        return obj.Guid != Guid.split("_")[1]
    })
    MultiFileSelectArray = MultiFileSelectArray.filter(function (obj) {
        return obj != Guid.split("_")[1]
    })
}
function ConfirmDelete() {
    let uploadClass = new FileUpload();
    uploadClass.DownloadDeleteMultiPel("Delete");
    var ele = document.getElementsByName("chk");
    for (var i = 0; i < ele.length; i++) {
        if (ele[i].type == 'checkbox')
            ele[i].checked = false;

    }
    document.getElementById('DeleteConfirmModal').style.display = "none";
}
function closeModel(flag) {
    switch (flag) {
        case "Delete":
            {
                var ele = document.getElementsByName("chk");
                for (var i = 0; i < ele.length; i++) {
                    if (ele[i].type == 'checkbox')
                        ele[i].checked = false;

                }
                debugger
                document.getElementById("DownloadSelectedFile").style.display = "none";
                document.getElementById("DeleteSelectedFile").style.display = "none";
                for (var i = 0; i < MultiFileSelectArray.length; i++) {
                    var row = document.getElementById("Table_" + MultiFileSelectArray[i]);
                    row.parentNode.removeChild(row);
                }
                document.getElementById('DeleteConfirmModal').style.display = "none";

                MultiFileSelectArray = [];
                MultiFileSelectArrayFileName = [];

                break;
            }
        case "Share":
            {
                document.getElementById('ShareFileLink').style.display = "none";
            }
    }
}
function checkValue(AwsId, guid, FileName) {
    debugger
    let uploadClass = new FileUpload();
    uploadClass.checkValue(AwsId, guid, FileName);
}
function PauseUploading(amazonunqID) {
    let uploadClass = new FileUpload();
    uploadClass.PauseUploading(amazonunqID);
}
function ResumeUploading(amazonunqID) {
    let uploadClass = new FileUpload();
    uploadClass.ResumeUploading(amazonunqID);
}
function ReTryUploading(amazonunqID) {
    let uploadClass = new FileUpload();
    uploadClass.ReTryUploading(amazonunqID);
}
function CancelUploading(awsid) {
    let uploadClass = new FileUpload();
    uploadClass.CancelUploading(awsid);
}