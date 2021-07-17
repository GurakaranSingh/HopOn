var _Guid = "";

window.refreshFileLink = () => {
    DotNet.invokeMethodAsync('HopOn', 'RefreshFileLink', _Guid)
        .then(data => {
            var files = document.getElementById("fileToUpload").files;

            var FileUploadClass = [];
            for (let i = 0; i < files.length; i++) {
                FileUploadClass[i] = new FileUpload();
                FileUploadClass[i].start_upload(files[i], Math.random());
            }
            document.getElementById('AllDownloadButton').style.display = "inline-block";
            document.getElementById('fileToUpload').value = null
        });
};

function GetFileList(Id, _type) {
    debugger
    var xhr = new XMLHttpRequest();
    xhr.onload = function () {
        if (xhr.status == 200) {
            document.getElementById("GeneratedLinkListRefresh").click();
        }
    };
    var obj = {
        FileId: Id.toString(),
        FileToken : "aGSHA",
        FileLink :"adssajhgd",
        Expired: false,
        Type: _type
    }
    
   // xhr.open("POST", "File/GenrateLink");
    xhr.open("POST", "api/Upload/GenrateLink");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.send(JSON.stringify(obj));
}
function ShareFile(Guid) {
    debugger
    //var url = window.location.href + "api/Upload/ShareFile/" + Guid
    //document.getElementById("ShareLink").value = url
    var filelistcomponnent = document.getElementById('shareTextBox')
    //filelistcomponnent.insertAdjacentHTML("afterend",
    //    '<button class="btn btn-outline-secondary" id="GeneratedLinkListRefresh_+"' + Guid + '" style="display:none" @onclick="() =>GeneratedLinkRefresh( ' + "'" + Guid + "'"+')">Display</button>'//GeneratedLinkRefresh(' + Guid + ')
    //);
    _Guid = Guid
    document.getElementById('ShareFileLink').style.display = "inline-block";
    document.getElementById("GeneratedLinkListRefresh_" + Guid).click();
}

function GetLink(select) {
    debugger
    GetFileList(_Guid, parseInt(select.value));
}