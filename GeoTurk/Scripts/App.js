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