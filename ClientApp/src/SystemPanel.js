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
            {logs.map((log, index) => (
                <div key={index} className="log-entry">{log}</div>
            ))}
        </div>
    );
}

export default SystemPanel;
