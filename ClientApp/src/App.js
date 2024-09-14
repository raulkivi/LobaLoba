import React, { useState, useEffect, useRef } from "react";
import {
    TextField,
    PrimaryButton,
    Stack,
    MessageBar,
    MessageBarType,
    IconButton,
} from "@fluentui/react";
import { initializeIcons } from '@fluentui/react/lib/Icons';
import axios from "axios";
import './App.css';

// Initialize Fluent UI icons
initializeIcons();

function App() {
    const [userMessage, setUserMessage] = useState("");
    const [conversationHistory, setConversationHistory] = useState([]);
    const [error, setError] = useState(null);
    const [isSoundOn, setIsSoundOn] = useState(false);
    const [isRecordingOn, setIsRecordingOn] = useState(false);
    const [isSystemMessagesOn, setIsSystemMessagesOn] = useState(false);
    const [isTyping, setIsTyping] = useState(false);
    const messageEndRef = useRef(null);
    const typingTimeoutRef = useRef(null);

    useEffect(() => {
        // Fetch initial button states from the API
        const fetchButtonStates = async () => {
            try {
                const response = await axios.get("/api/chat/buttonstates");
                setIsSoundOn(response.data.isSoundOn);
                setIsRecordingOn(response.data.isRecordingOn);
                setIsSystemMessagesOn(response.data.isSystemMessagesOn);
            } catch (err) {
                setError("Error fetching initial button states");
            }
        };

        fetchButtonStates();
    }, []);

    const handleSendMessage = async () => {
        if (!userMessage) return;

        try {
            const response = await axios.post("/api/Chat", {
                text: userMessage,
            });

            setConversationHistory(response.data.conversationHistory);
            setUserMessage("");
            setIsTyping(false); // Hide typing bubble when message is sent
        } catch (err) {
            setError("Error communicating with the chatbot API");
        }
    };

    const updateButtonStates = async (newButtonStates) => {
        try {
            await axios.post("/api/chat/buttonstates", newButtonStates);
        } catch (err) {
            setError("Error updating button states");
        }
    };

    const handleSoundToggle = () => {
        const newSoundState = !isSoundOn;
        setIsSoundOn(newSoundState);
        updateButtonStates({
            isSoundOn: newSoundState,
            isRecordingOn,
            isSystemMessagesOn
        });
    };

    const handleRecordingToggle = () => {
        const newRecordingState = !isRecordingOn;
        setIsRecordingOn(newRecordingState);
        updateButtonStates({
            isSoundOn,
            isRecordingOn: newRecordingState,
            isSystemMessagesOn
        });
    };

    const handleSystemMessagesToggle = () => {
        const newSystemMessagesState = !isSystemMessagesOn;
        setIsSystemMessagesOn(newSystemMessagesState);
        updateButtonStates({
            isSoundOn,
            isRecordingOn,
            isSystemMessagesOn: newSystemMessagesState
        });
    };

    const handleUserTyping = (e, newValue) => {
        setUserMessage(newValue || "");
        setIsTyping(true);

        // Clear the previous timeout
        if (typingTimeoutRef.current) {
            clearTimeout(typingTimeoutRef.current);
        }

        // Set a new timeout to hide the typing bubble after 1 second of inactivity
        typingTimeoutRef.current = setTimeout(() => {
            setIsTyping(false);
        }, 1000);
    };

    useEffect(() => {
        if (messageEndRef.current) {
            messageEndRef.current.scrollIntoView({ behavior: "smooth" });
        }
    }, [conversationHistory]);

    return (
        <div style={{ padding: 20 }}>
            <h2>🚌</h2>

            {error && (
                <MessageBar messageBarType={MessageBarType.error}>{error}</MessageBar>
            )}

            <div className="message-container" style={{ marginBottom: 20 }}>
                {conversationHistory.map((msg, index) => (
                    <div key={index}
                        className={`message-bubble ${msg.role === "user" ? "user-message" : "bot-message"}`}>
                        <p style={{ whiteSpace: 'pre-wrap' }}>{msg.text}</p>
                    </div>
                ))}
                {isTyping && (
                    <div className="message-bubble typing-bubble">
                        <p>...</p>
                    </div>
                )}
                <div ref={messageEndRef} />
            </div>

            <Stack horizontal tokens={{ childrenGap: 10 }}>
                <TextField
                    label="Your request"
                    value={userMessage}
                    onChange={handleUserTyping}
                    multiline
                    style={{ width: 300, height: 60 }}
                />
                <PrimaryButton text="Send" onClick={handleSendMessage} />
            </Stack>

            <Stack horizontal tokens={{ childrenGap: 10 }} style={{ marginTop: 10 }}>
                <IconButton
                    iconProps={{ iconName: isSoundOn ? 'Volume3' : 'Volume0' }} // Sound on/off icon
                    title="Sound On/Off"
                    ariaLabel="Sound On/Off"
                    onClick={handleSoundToggle}
                />
                <IconButton
                    iconProps={{ iconName: isRecordingOn ? 'Microphone' : 'MicOff2' }} // Recording on/off icon
                    title="Recording On/Off"
                    ariaLabel="Recording On/Off"
                    onClick={handleRecordingToggle}
                />
                <IconButton
                    iconProps={{ iconName: isSystemMessagesOn ? 'MessageFill' : 'Message' }} // System messages on/off icon
                    title="System Messages On/Off"
                    ariaLabel="System Messages On/Off"
                    onClick={handleSystemMessagesToggle}
                />
            </Stack>
        </div>
    );
}

export default App;
