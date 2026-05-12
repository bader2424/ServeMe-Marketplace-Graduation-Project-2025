function sendMessage() {
    const chatMessages = document.getElementById("chatMessages");
    const messageInput = document.getElementById("messageInput");
    const messageText = messageInput.value.trim();

    if (messageText) {
        const messageElement = document.createElement("div");
        messageElement.classList.add("message", "message-customer");
        messageElement.textContent = messageText;

        chatMessages.appendChild(messageElement);

        messageInput.value = "";
        messageInput.focus();

        chatMessages.scrollTop = chatMessages.scrollHeight;
    }
}

const chatMessages = document.getElementById("chatMessages");
chatMessages.scrollTop = chatMessages.scrollHeight;