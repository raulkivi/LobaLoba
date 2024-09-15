import React, { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

function SystemPanel() {
    const [logs, setLogs] = useState([]);

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

    return (
        <div>
            <h2>System Logs</h2>
            <ul>
                {logs.map((log, index) => (
                    <li key={index}>{log}</li>
                ))}
            </ul>
        </div>
    );
}

export default SystemPanel;
