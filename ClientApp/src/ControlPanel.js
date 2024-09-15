// ClientApp/src/ControlPanel.js
import React from "react";
import { IconButton, Stack } from "@fluentui/react";

const ControlPanel = ({
    isSoundOn,
    isRecordingOn,
    isSystemMessagesOn,
    handleSoundToggle,
    handleRecordingToggle,
    handleSystemMessagesToggle
}) => {
    return (
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
    );
};

export default ControlPanel;
