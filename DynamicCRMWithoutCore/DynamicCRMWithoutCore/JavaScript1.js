function alertJS() {
    alert('Hello , this is account test alert');
    var phone = Xrm.Page.getAttribute("telephone1").getValue();
    alert("Phone is : " + phone);
    Xrm.Page.getAttribute("fax").setValue(phone);


}

function showMessage(executionContext) {
    var form_contex = executionContext.getFormContext();
    form_contex.ui.setFormNotification("Hello" + phone, "INFO", "ID1");
    form_contex.ui.setFormNotification("Hello" + phone, "WARNING", "ID2");
    form_contex.ui.setFormNotification("Hello" + phone, "ERROR", "ID3");

}

function clearAllNot(executionContext) {
    var form_contex = executionContext.getFormContext();
    form_contex.ui.clearFormNotification("ID1");
    form_contex.ui.clearFormNotification("ID2");
    form_contex.ui.clearFormNotification("ID3");
}

function tabEvent() {
    alert('test');
}
UOP_JS