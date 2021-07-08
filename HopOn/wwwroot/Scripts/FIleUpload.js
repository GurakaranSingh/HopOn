
window.returnArrayAsync = () => {
    DotNet.invokeMethodAsync('HopOn', 'ReturnArrayAsync')
        .then(data => {
            var files = document.getElementById("fileToUpload").files;
            var FileUploadClass = [];
            for (let i = 0; i < files.length; i++) {
                FileUploadClass[i] = new FileUpload();
                FileUploadClass[i].start_upload(files[i], i);
            }
            document.getElementById('fileToUpload').value = null
        });
};
function upload_file(start, amazonunqID, index) {
    var uploadClass = [];
    uploadClass[index] = new FileUpload();
    uploadClass[index].upload_file(start, amazonunqID);
}

function DeleteFile(FileName) {
    let uploadClass = new FileUpload();
    uploadClass.DeleteFile(FileName);
}
function DownloadDeleteMultiPel(Flag) {
    debugger
    let uploadClass = new FileUpload();
    uploadClass.DownloadDeleteMultiPel(Flag);
}
function checkValue(AwsId, guid) {
    debugger
    let uploadClass = new FileUpload();
    uploadClass.checkValue(AwsId, guid);
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