function closeDialog(elName) {
    let dialog = document.getElementById(elName);
    dialog.close();
}

function showDialog(elName) {
    let dialog = document.getElementById(elName);
    dialog.showModal();
}