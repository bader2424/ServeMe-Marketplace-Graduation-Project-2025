function submitForm() {
    const serviceName = document.getElementById('serviceName').value;
    const servicePrice = document.getElementById('servicePrice').value;
    const serviceDetails = document.getElementById('serviceDetails').value;
    const serviceType = document.getElementById('serviceType').value;
    const availableTime = document.getElementById('availableTime').value;
    const userName = document.getElementById('userName').value;
    const phone = document.getElementById('phone').value;
    const email = document.getElementById('email').value;

    if (serviceName && servicePrice && serviceDetails && serviceType && availableTime && userName && phone && email) {
        alert("تمت إضافة الخدمة بنجاح!");
        document.getElementById("serviceForm").reset();
    } else {
        alert("يرجى تعبئة جميع الحقول.");
    }
}