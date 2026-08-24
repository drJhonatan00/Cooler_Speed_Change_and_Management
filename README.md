# Cooler Speed Change & Management

A high-performance C#/.NET Windows Forms application designed for real-time hardware temperature monitoring and dynamic PWM fan speed control. Powered by *LibreHardwareMonitor*, this application delivers precise hardware telemetry while maintaining zero UI freezing and extremely low CPU overhead.

<img width="829" height="343" alt="Imagem13" src="https://github.com/user-attachments/assets/e6de2690-089f-4a0a-8725-1962f831d8c8" />

---

## Key Features

* *Real-Time Temperature Monitoring:* Continuous tracking and live graphing of CPU and GPU core temperatures.
* *Dynamic Fan Control:* Individual selection and manual PWM control for compatible cooling fans connected to the motherboard or GPU.
* *Synchronized Charting:* Dual real-time time-series charts displaying temperature (°C) and fan rotation speed (RPM) in sync.
* *Full System Diagnostics:* Built-in hardware scanner that generates a comprehensive telemetry report of all detected sensors (volts, watts, clock, load).
* *Asynchronous & Non-Blocking:* Engineered with C# async/await and background tasks (Task.Run) to prevent UI lockups and ensure minimal resource consumption.
* *Automatic Safeguards:* Restores hardware default fan control curves automatically upon application exit.

<img width="355" height="978" alt="Imagem15" src="https://github.com/user-attachments/assets/f04af90d-2041-4489-9b68-68815476ea2a" />


---

## Prerequisites

* *Operating System:* Windows 10 / 11 (64-bit)
* *Framework:* .NET Framework 4.8 / .NET 6.0 or higher
* *Permissions:* *Administrator Privileges* required to interface with motherboard and GPU kernel drivers.

---

## Languages

* English
* Español
* Português
* 한국인
* 日本語
* Deutsch
* Italiano
* Français
* 简体中文

<img width="263" height="399" alt="Imagem14" src="https://github.com/user-attachments/assets/29c458f4-9979-43b4-94b6-86810d8f4052" />


---

## Tech Stack

* *Language:* C#
* *UI Framework:* Windows Forms (WinForms)
* *Hardware API:* [LibreHardwareMonitor](https://github.com/LibreHardwareMonitor/LibreHardwareMonitor)
* *Data Visualization:* System.Windows.Forms.DataVisualization.Charting

---

## Installation & Usage

1. Clone or download this repository.
2. Open the solution file (.sln) in *Visual Studio*.
3. Build the project in Release mode.
4. *Important:* Run the resulting executable as *Administrator* (right-click $\rightarrow$ Run as administrator).
5. Select your target fan device from the drop-down menu, adjust the control slider, and monitor real-time RPM and temperature changes.

---

## Security & Execution Level

Because hardware control APIs require direct interaction with low-level kernel drivers, this application is configured to request elevated privileges
