# Role Workflow Flowcharts
## NRL Conference Hall Booking System

> All flowcharts use standard notation:
> **([ ])** = Start / End / Connectors
> **[ ]** = Process / Action
> **{ }** = Decision / Logic Check
> **[/ /]** = Input / Output / Data Read/Write

---

##### START OF USER FLOWCHART
## 1. USER — Booking a Conference Hall

```mermaid
---
title: USER — Booking a Conference Hall
---
flowchart TD
    A([START]) --> B[/Open Browser & Go to System URL/]
    B --> C[/Enter Windows Username & Password/]
    C --> D{Valid Credentials?}

    D -- No --> E[/Show Error: Invalid Credentials/]
    E --> C

    D -- Yes --> F{Is First-Time Login?}
    F -- Yes --> G[System Auto-Creates User Account\nFetches Name & Dept from Company Server]
    F -- No --> H[System Updates Profile\nfrom Company Server]
    
    G --> I[Redirect to Home Dashboard]
    H --> I

    I --> J{What does user want to do?}

    %% Path: View Bookings
    J -- "View My Bookings" --> K[Click 'My Bookings']
    K --> L[/System Displays List of Bookings with Status/]
    L --> M{Does User Want to Cancel a Booking?}
    M -- Yes --> N{Is Booking Still Upcoming?}
    N -- Yes --> O[Click 'Cancel']
    N -- No --> P[/Cancellation Blocked/]
    O --> Q[/Booking Status Marked 'Cancelled'/]
    Q --> Z([END])
    M -- No --> Z
    P --> Z

    %% Path: Make a Booking
    J -- "Make a Booking" --> R[Click 'Book a Room' or Use Calendar]
    R --> S[/Select Conference Room (Searchable Dropdown)/]
    S --> T[/Select Cost Centre (Searchable Dropdown)/]
    T --> U[/Enter Date, Start Time, End Time/]
    U --> V{Is Time Slot Available?}
    V -- "No (Conflict Detected)" --> W[/Display Conflict Warning/]
    W --> U
    
    V -- "Yes" --> X[/Enter Purpose & Expected Attendees/]
    X --> Y{Need IT or AV Equipment?}
    Y -- Yes --> AA[/Fill IT Requirements (Video Conf, Projector, etc.)/]
    AA --> AB[Submit Booking]
    Y -- No --> AB
    
    AB --> AC[/Booking Created:\nStatus = Pending/]
    AC --> AD[/System Notifies Assigned Allocator(s)/]
    
    AD --> AE{Allocator Decision?}
    AE -- Approved --> AF[/Status = Approved\nUser Notified/]
    AE -- Rejected --> AG[/Status = Rejected\nReason Shown to User/]
    
    AF --> Z
    AG --> Z

    %% Styling
    classDef startEnd fill:#d5e8d4,stroke:#82b366,stroke-width:2px,color:#000;
    classDef process fill:#dae8fc,stroke:#6c8ebf,stroke-width:2px,color:#000;
    classDef decision fill:#ffe6cc,stroke:#d79b00,stroke-width:2px,color:#000;
    classDef io fill:#e1d5e7,stroke:#9673a6,stroke-width:2px,color:#000;
    class A,Z startEnd;
    class G,H,I,K,O,R,AB process;
    class D,F,J,M,N,V,Y,AE decision;
    class B,C,E,L,P,Q,S,T,U,W,X,AA,AC,AD,AF,AG io;
```
##### END OF USER FLOWCHART

---

##### START OF ALLOCATOR FLOWCHART
## 2. ALLOCATOR — Managing & Approving Bookings (Hall-Specific)

> **Key Rule:** Each Allocator is assigned to specific halls by an Admin. They ONLY see bookings for their assigned halls.

```mermaid
---
title: ALLOCATOR — Managing & Approving Bookings (Hall-Specific)
---
flowchart TD
    A([START]) --> B[/Log in as Allocator/]
    B --> C[Land on Allocator Dashboard]
    C --> F[/View Bookings Filtered by Pre-Assigned Halls\n(Assigned during Account Creation)/]
    
    F --> G{Are There Pending Bookings?}

    G -- No --> H[/Dashboard Shows: No Action Required/]
    H --> Z

    G -- Yes --> I[Click on a Pending Booking for Details]
    I --> J{Is Booking For an Assigned Hall?}
    J -- No --> K[/Error: 403 Forbidden/]
    K --> Z

    J -- Yes --> L[/Review Details: Time, Purpose, Attendees/]
    L --> M{Is Booking Valid & No Conflicts?}

    M -- "No (Reject)" --> N[Click Reject]
    N --> O[/Enter Rejection Reason/]
    O --> P[/Status = Rejected\nUser Notified/]
    P --> Q{More Pending Bookings?}

    M -- "Yes (Approve)" --> R[Click Approve]
    R --> S{Does Booking Have IT/AV Requirements?}

    S -- Yes --> T[/Status = Approved\nFlag for ITFM Team\nNotify User & ITFM/]
    T --> Q

    S -- No --> U[/Status = Approved\nNotify User/]
    U --> Q

    Q -- Yes --> I
    Q -- No --> Z

    %% Styling
    classDef startEnd fill:#d5e8d4,stroke:#82b366,stroke-width:2px,color:#000;
    classDef process fill:#dae8fc,stroke:#6c8ebf,stroke-width:2px,color:#000;
    classDef decision fill:#ffe6cc,stroke:#d79b00,stroke-width:2px,color:#000;
    classDef io fill:#e1d5e7,stroke:#9673a6,stroke-width:2px,color:#000;
    class A,Z startEnd;
    class C,I,N,R process;
    class G,J,M,Q,S decision;
    class B,F,H,K,L,O,P,T,U io;
```
##### END OF ALLOCATOR FLOWCHART

---

##### START OF ITFM FLOWCHART
## 3. ITFM — IT & Facilities Management

```mermaid
---
title: ITFM — IT & Facilities Management
---
flowchart TD
    A([START]) --> B[/Log in as ITFM/]
    B --> C[Land on ITFM Dashboard]
    C --> D[/System Displays Approved Bookings with IT Requirements/]
    D --> E{Any Pending IT Tasks?}

    E -- No --> F[/Dashboard Shows: No IT Tasks Pending/]
    F --> Z([END])

    E -- Yes --> G[Click on a Booking to View IT Details]
    G --> H[/Review Specific Needs (Laptop, Mic, etc.)/]
    H --> I[Physically Prepare Equipment in the Room]
    
    I --> J{Is Equipment Ready and Tested?}

    J -- "No (Issue Found)" --> K[Click 'Mark NOT Ready']
    K --> L[/Log the Issue/Reason/]
    L --> M[/System Flags Booking\nNotifies Admin & Allocator/]
    M --> N{More Bookings to Handle?}

    J -- "Yes (Ready)" --> O[Click 'Mark Ready']
    O --> P[/Booking IT Status Updated to Ready/]
    P --> N

    N -- Yes --> G
    N -- No --> Z

    %% Styling
    classDef startEnd fill:#d5e8d4,stroke:#82b366,stroke-width:2px,color:#000;
    classDef process fill:#dae8fc,stroke:#6c8ebf,stroke-width:2px,color:#000;
    classDef decision fill:#ffe6cc,stroke:#d79b00,stroke-width:2px,color:#000;
    classDef io fill:#e1d5e7,stroke:#9673a6,stroke-width:2px,color:#000;
    class A,Z startEnd;
    class C,I,K,O process;
    class E,J,N decision;
    class B,D,F,G,H,L,M,P io;
```
##### END OF ITFM FLOWCHART

---

##### START OF ADMIN FLOWCHART
## 4. ADMIN — Full System Administration

```mermaid
---
title: ADMIN — Full System Administration
---
flowchart TD
    A([START]) --> B[/Log in as Admin/]
    B --> C[Land on Admin Dashboard]
    C --> D{Select Administrative Task}

    %% Manage Users
    D -- "Manage Users" --> E[Go to Users Panel]
    E --> F{Select Action}
    
    F -- "Create System Account" --> G[/Enter Details (Username, Role, Dept)/]
    G --> H{Is Role Allocator?}
    H -- Yes --> I[/Assign Conference Halls (Checkbox List)/]
    I --> J[Save Account]
    H -- No --> J
    J --> K[/Account Created/]
    K --> E
    
    F -- "Edit Role/Status" --> L[Select User]
    L --> M{What to Edit?}
    M -- "Change Role" --> N[/Select New Role/]
    M -- "Deactivate" --> O[/Lock Account/]
    M -- "Reactivate" --> P[/Unlock Account/]
    M -- "Manage Allocator Halls" --> Q[/Update Assigned Halls/]
    N --> E
    O --> E
    P --> E
    Q --> E

    %% Manage Halls
    D -- "Manage Halls" --> R[Go to Halls Panel]
    R --> S{Select Action}
    S -- "Add Hall" --> T[/Enter Hall Details & Capacity/]
    T --> U[/Select Cost Centre (Searchable Dropdown) & Dept/]
    U --> V[System Auto-Generates HallCode]
    V --> W[/Save Hall to Database/]
    W --> R
    S -- "Edit Hall" --> X[/Update Details/] --> R
    S -- "Toggle Status" --> Y[/Mark Hall Active/Inactive/] --> R

    %% Manage Cost Centres
    D -- "Manage Cost Centres" --> CA[Go to Cost Centres Panel]
    CA --> CB{Select Action}
    CB -- "Add Cost Centre" --> CC[/Enter Code & Name/] --> CD[/Save to Database/] --> CA
    CB -- "Edit Cost Centre" --> CE[/Update Name/Description/] --> CA
    CB -- "Toggle Status" --> CF[/Mark Active/Inactive/] --> CA

    %% View Booking History / Block Halls
    D -- "Booking History" --> BA[Go to Booking History]
    BA --> BB[/View All System Bookings/]
    BB --> BC{Cancel a Booking?}
    BC -- Yes --> BD[Click Cancel] --> BE[/Booking Cancelled\nAudit Log Created/] --> BA
    BC -- No --> BA

    D -- "Logout" --> Z([END])

    %% Styling
    classDef startEnd fill:#d5e8d4,stroke:#82b366,stroke-width:2px,color:#000;
    classDef process fill:#dae8fc,stroke:#6c8ebf,stroke-width:2px,color:#000;
    classDef decision fill:#ffe6cc,stroke:#d79b00,stroke-width:2px,color:#000;
    classDef io fill:#e1d5e7,stroke:#9673a6,stroke-width:2px,color:#000;
    class A,Z startEnd;
    class C,E,J,L,R,V process;
    class D,F,H,M,S,CB,BC decision;
    class B,G,I,K,N,O,P,Q,T,U,W,X,Y,CA,CC,CD,CE,CF,BA,BB,BD,BE io;
```
##### END OF ADMIN FLOWCHART

---

##### START OF AUTHENTICATION FLOWCHART
## 5. AUTHENTICATION FLOW — System Wide

```mermaid
---
title: AUTHENTICATION FLOW — System Wide
---
flowchart TD
    A([USER/ADMIN/ITFM OPENS SYSTEM]) --> B[/Enter Windows Username & Password/]
    B --> C[Click Sign In]
    C --> D{Check Local System Database}

    D -- "Found & Password Matches" --> E[/User Authenticated via Local DB/]
    E --> F([REDIRECT TO DASHBOARD])

    D -- "Not Found or Incorrect" --> G{Check Company Directory Server\n(via CompanyAuthService)}

    G -- "Credentials Invalid" --> H[/Show Error: Invalid Username or Password/]
    H --> B

    G -- "Credentials Valid" --> I{Does Local Account Exist?}

    I -- "No (First Login)" --> J[System Auto-Creates Local Account]
    J --> K[/Extract Name & Department from Server/]
    K --> L[/Save as Default 'User' Role/]
    L --> F

    I -- "Yes (Returning User)" --> M[System Updates Local Profile]
    M --> N[/Sync Name & Department Changes from Server/]
    N --> F

    %% Styling
    classDef startEnd fill:#d5e8d4,stroke:#82b366,stroke-width:2px,color:#000;
    classDef process fill:#dae8fc,stroke:#6c8ebf,stroke-width:2px,color:#000;
    classDef decision fill:#ffe6cc,stroke:#d79b00,stroke-width:2px,color:#000;
    classDef io fill:#e1d5e7,stroke:#9673a6,stroke-width:2px,color:#000;
    class A,F startEnd;
    class C,J process;
    class D,G,I decision;
    class B,E,H,K,L,M,N io;
```
##### END OF AUTHENTICATION FLOWCHART

