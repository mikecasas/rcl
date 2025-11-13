window.ShowDialog = function () {
    document.getElementById('my-dialog').showModal();

    if (dialog.open) {
        console.log("Dialog open");
    } else {
        console.log("Dialog closed");
    }
}