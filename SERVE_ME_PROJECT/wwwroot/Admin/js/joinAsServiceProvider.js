function submitProviderForm() {
    const providerName = document.getElementById('providerName').value;
    const providerPhone = document.getElementById('providerPhone').value;
    const providerEmail = document.getElementById('providerEmail').value;
    const providerLocation = document.getElementById('providerLocation').value;
    const servicesProvided = document.getElementById('servicesProvided').value;
    const serviceAvailability = document.getElementById('serviceAvailability').value;
    const providerDetails = document.getElementById('providerDetails').value;

    if (providerName && providerPhone && providerEmail && providerLocation && servicesProvided && serviceAvailability && providerDetails) {
        alert("تم إرسال الطلب بنجاح!");
        document.getElementById("providerForm").reset();
    } else {
        alert("يرجى تعبئة جميع الحقول.");
    }
}