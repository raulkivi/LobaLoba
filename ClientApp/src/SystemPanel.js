import React, { useEffect, useState, useRef } from "react";
import * as signalR from "@microsoft/signalr";
import './App.css'; // Ensure this import is present to apply the CSS

function SystemPanel() {
    const [logs, setLogs] = useState([]);
    const endOfLogsRef = useRef(null);

    useEffect(() => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/logHub")
            .build();

        connection.on("ReceiveLog", (message) => {
            setLogs((prevLogs) => [...prevLogs, message]);
        });

        connection.start().catch((err) => console.error(err.toString()));

        return () => {
            connection.stop();
        };
    }, []);

    useEffect(() => {
        if (endOfLogsRef.current) {
            endOfLogsRef.current.scrollIntoView({ behavior: "smooth" });
        }
    }, [logs]);

    return (
        <div className="system-messages-container">
            {logs.map((log, index) => (
                <div key={index} className="log-entry">{log}</div>
            ))}
            <div ref={endOfLogsRef} />
        </div>
    );
}

export default SystemPanel;
