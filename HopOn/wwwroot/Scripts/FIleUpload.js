////window.returnArrayAsync = () => {
////    DotNet.invokeMethodAsync('HopOn', 'ReturnArrayAsync')
////        .then(data => {
////            closemodel();
////            var files = document.getElementById("fileToUpload").files;

////            var FileUploadClass = [];
////            for (let i = 0; i < files.length; i++) {
////                FileUploadClass[i] = new FileUpload();
////                FileUploadClass[i].start_upload(files[i], Math.random());
////            }
////            document.getElementById('AllDownloadButton').style.display = "inline-block";
////            document.getElementById('fileToUpload').value = null
////        });
////};
function uploadfile() {
    var files = document.getElementById("fileToUpload").files;
    var FileUploadClass = [];
    for (let i = 0; i < files.length; i++) {
        FileUploadClass[i] = new FileUpload();
        FileUploadClass[i].start_upload(files[i], Math.random());
    }
    //document.getElementById('AllDownloadButton').style.display = "inline-block";
    document.getElementById('AllDownloadButton').click();
    document.getElementById('fileToUpload').value = null
}
function closemodel() {
    window.returnArrayAsync = () => {
        DotNet.invokeMethodAsync('HopOn', 'CloseOpenModel')
            .then(data => {
            });
    };
}
function Resume(amazonunqID, nextslice,index) {
     
    var file = Selectedfile.find(obj => {
        return obj.AmazonID === amazonunqID
    });
    file.IsStop = false;
    document.getElementById("pause_" + amazonunqID).style.display = "inline-block";
    document.getElementById("resume_" + amazonunqID).style.display = "none";
    new FileUpload().ResumeFile(nextslice, amazonunqID, index);
}
function upload_file(start, amazonunqID, index) {
    var uploadClass = [];
    uploadClass[index] = new FileUpload();
    uploadClass[index].upload_file(start, amazonunqID, index);
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

    let uploadClass = new FileUpload();
    if (Flag == "Delete") {
        var filelistcomponnent = document.getElementById('FirstHead')
        for (var i = 0; i < MultiFileSelectArrayFileName.length; i++) {
            var Guid = "'" + "Table_" + MultiFileSelectArrayFileName[i].Guid + "'";
            filelistcomponnent.insertAdjacentHTML("afterend",
                '<tr id = ' + Guid + '>' +
                '<td style="border: 1px solid #ddd;padding: 8px;"> ' + MultiFileSelectArrayFileName[i].FileName + '</td >' +
                '<td>' +
                '<a href="#" onclick="RemoveItemFromList(' + Guid + ')"> <img src="Images/delete-icon.svg" style="fill:#df1b1b; width:20px; height:20px;"/></a>' +
                '</td>' +
                '</tr>');
        }
        document.getElementById('DeleteConfirmModal').style.display = "inline-block";
    }
    else {
        document.getElementById('DeleteConfirmModal').style.display = "none";
        uploadClass.DownloadDeleteMultiPel(Flag);
    }
    document.getElementById("RefreshQuota").click();
}

function RemoveItemFromList(Guid) {

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
    document.getElementById("RefreshQuota").click();
    location.reload();
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
    location.reload();
}
function checkValue(AwsId, guid, FileName) {

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

function ShowIcon(guid) {

    var kebab = document.getElementById('k_' + guid), // document.querySelector('.kebab' + guid),
        middle = document.getElementById('m_' + guid), //document.querySelector('.middle'),
        cross = document.getElementById('c_' + guid), // document.querySelector('.cross'),
        dropdown = document.getElementById('d_' + guid); //document.querySelector('.dropdown');

    kebab.addEventListener('click', function () {

        middle.classList.toggle('active');
        cross.classList.toggle('active');
        dropdown.classList.toggle('active');
    })
    kebab.click();
}

function downloadFromUrl(url, fileName) {
    debugger
    document.getElementById("RefreshQuota").click();
    const anchorElement = document.createElement('a');
    anchorElement.href = url.url;
    anchorElement.download = url.fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
}
function downloadFromByteArray(byteArray, fileName, contentType) {
     
    // Convert base64 string to numbers array.
    const numArray = byteArray;//atob(byteArray).split('').map(c => c.charCodeAt(0));

    // Convert numbers array to Uint8Array object.
    const uint8Array = new Uint8Array(numArray);

    // Wrap it by Blob object.
    const blob = new Blob([uint8Array], { type: byteArray.contentType });

    // Create "object URL" that is linked to the Blob object.
    const url = URL.createObjectURL(blob);
    
    // Invoke download helper function that implemented in 
    // the earlier section of this article.
    downloadFromUrl(url,byteArray.fileName );

    // At last, release unused resources.
    URL.revokeObjectURL(url);
}
