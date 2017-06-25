var answerTypes = {
    FreeText: 1,
    FileUpload: 2,
    ChoiseText: 3,
    ChoiseImage: 4
}

function showToastrOnFormSuccess(data) {
    if (!data){
        return;
    }

    if (data.success) {
        toastr.success(data.message);
    }
    else {
        toastr.error(data.message);
    }
}