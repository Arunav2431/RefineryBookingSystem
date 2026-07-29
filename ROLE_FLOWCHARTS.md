# Role Workflow Flowcharts
## NRL Conference Hall Booking System

> All flowcharts use standard notation:
> **Ovals** = Start / End &nbsp;|&nbsp; **Rectangles** = Process &nbsp;|&nbsp; **Diamonds** = Decision &nbsp;|&nbsp; **Parallelograms** = Input / Output

---

## 1. USER — Booking a Conference Hall

```mermaid
flowchart TD
    A([START]) --> B[/Open browser &\ngo to system URL/]
    B --> C[Enter Windows Username\n+ Password]
    C --> D{Valid credentials?}

    D -- No --> E[/Show error:\nInvalid credentials/]
    E --> C

    D -- Yes: First-ever login --> F[System auto-creates\nUser account\nName & Dept from company server]
    D -- Yes: Returning user --> G[Profile refreshed\nfrom company server]
    F --> H[Redirect to Home Dashboard]
    G --> H

    H --> I{What does user want?}

    I -- View my bookings --> J[Click My Bookings]
    J --> K[/List of all bookings\nwith status/]
    K --> L{Cancel a booking?}
    L -- Yes --> M[Click Cancel on\nan upcoming booking]
    M --> N[/Booking marked Cancelled/]
    N --> Z([END])
    L -- No --> Z

    I -- Make a booking --> O[Click Book a Room\nor use Calendar]
    O --> P[/Select Conference Room/]
    P --> Q[/Enter Date, Start Time,\nEnd Time/]
    Q --> R{Time slot available?}
    R -- No: Conflict detected --> S[/Show conflict warning/]
    S --> Q
    R -- Yes --> T[/Enter Purpose &\nExpected Attendees/]
    T --> U{Need IT or AV\nequipment?}
    U -- Yes --> V[/Fill IT Requirements form:\nLaptop, Projector, Mic,\nVideo Conf, Recording/]
    V --> W[Submit Booking]
    U -- No --> W
    W --> X[/Booking created:\nStatus = Pending/]
    X --> Y[/Await Allocator Approval\nNotification sent/]
    Y --> AA{Allocator decision?}
    AA -- Approved --> AB[/Status = Approved\nUser can attend/]
    AA -- Rejected --> AC[/Status = Rejected\nReason shown to user/]
    AB --> Z
    AC --> Z
```

---

## 2. ALLOCATOR — Managing & Approving Bookings (Hall-Specific)

> **Key Rule:** Each Allocator is assigned to specific halls by Admin at account creation.
> They ONLY see bookings for their assigned halls. One hall can have multiple allocators.

```mermaid
flowchart TD
    A([START]) --> B[Log in with\nWindows Username + Password]
    B --> C[Land on Allocator Dashboard]
    C --> D{Has hall\nassignments?}

    D -- No: Admin has not assigned halls --> E[/Show warning:\nContact Admin to assign halls/]
    E --> Z([END])

    D -- Yes --> F[/View ONLY bookings for\nassigned halls\nFiltered automatically/]
    F --> G{Any bookings\npending review?}

    G -- No --> H[/Dashboard shows:\nAll clear for assigned halls/]
    H --> Z

    G -- Yes --> I[Click on a booking\nto view Details]
    I --> J{Booking is for\nan assigned hall?}
    J -- No: Access denied --> K[/403 Forbidden\nNot your assigned hall/]
    K --> Z

    J -- Yes --> L[/Read booking details:\nRoom, Date, Time,\nPurpose, Attendees/]
    L --> M{Is there a\ntime/room conflict?}

    M -- Yes --> N[Reject the booking]
    N --> O[/Enter rejection reason\ne.g. Double booking/]
    O --> P[/Status = Rejected\nUser notified/]
    P --> Q{More pending\nbookings?}

    M -- No --> R{Booking valid\nand appropriate?}
    R -- No --> N

    R -- Yes --> S[Click Approve]
    S --> T{Does booking have\nIT/AV requirements?}

    T -- Yes --> U[/System flags booking\nfor ITFM team/]
    U --> V[/Status = Approved\nIT ticket created\nITFM notified/]
    V --> Q

    T -- No --> W[/Status = Approved\nUser notified/]
    W --> Q

    Q -- Yes --> I
    Q -- No --> X[Block a room\nfor maintenance?]
    X -- Yes --> Y[Go to Hall Blocks]
    Y --> AA[/Select room, date range,\nenter reason/]
    AA --> AB[/Room blocked:\nno bookings allowed\nin this period/]
    AB --> Z
    X -- No --> Z
```

---

## 3. ITFM — IT & Facilities Management

```mermaid
flowchart TD
    A([START]) --> B[Log in with\nWindows Username + Password]
    B --> C[Land on ITFM Dashboard]
    C --> D[/View list of Approved bookings\nthat have IT requirements/]
    D --> E{Any bookings\nwith IT needs?}

    E -- No --> F[/Dashboard shows:\nNo IT tasks pending/]
    F --> Z([END])

    E -- Yes --> G[Click on a booking\nto view IT Details]
    G --> H[/Read IT Requirements:\nLaptop needed?\nProjector needed?\nVideo Conferencing?\nMicrophone?\nRecording?\nSpecial Instructions?/]

    H --> I[Physically prepare the\nrequested equipment\nin the conference room]
    I --> J{Equipment ready &\nall items confirmed?}

    J -- No: Issue found --> K[/Click Mark NOT Ready\nLog the issue/]
    K --> L[/Booking flagged:\nITFM notified Admin\nand Allocator/]
    L --> M{More bookings\nto handle?}

    J -- Yes --> N[/Click Mark Ready/]
    N --> O[/Booking status updated:\nIT confirmed as ready/]
    O --> P[/User and Allocator\ncan see IT is prepared/]
    P --> M

    M -- Yes --> G
    M -- No --> Z
```

---

## 4. ADMIN — Full System Administration

```mermaid
flowchart TD
    A([START]) --> B[Log in as sys.admin\nor any Admin account]
    B --> C[Admin Dashboard]
    C --> D{Which task?}

    D -- Manage Users --> E[Go to Admin Panel > Users]
    E --> F[/View all users:\nName, Username, Role, Status/]
    F --> G{Action needed?}

    G -- Create new\nAdmin/ITFM/Allocator --> H[Click Create System Account]
    H --> I[/Enter: Full Name,\nWindows Username,\nBadge ID, Department,\nRole, Initial Password/]
    I --> J[/Account created:\nPerson can log in immediately/]
    J --> K{More user tasks?}

    G -- Change someone's role --> L[Select new role\nfrom dropdown]
    L --> M[/Role updated\nAccess changes immediately/]
    M --> K

    G -- Deactivate user --> N[Click Deactivate]
    N --> O[/Account locked:\nUser cannot log in/]
    O --> K

    G -- Reactivate user --> P[Click Reactivate]
    P --> Q[/Account unlocked:\nUser can log in again/]
    Q --> K

    K -- Yes --> F
    K -- No --> D

    D -- Booking History --> R[Go to Admin > Booking History]
    R --> S[/View all bookings:\nsearchable by hall name,\npaginated list/]
    S --> T{Cancel a booking?}
    T -- Yes --> U[Click Cancel on booking]
    U --> V[/Booking cancelled\nAudit log created\nUser can see status change/]
    V --> D
    T -- No --> D

    D -- Block a Hall --> W[Go to Hall Blocks > Create]
    W --> X[/Select room, date range,\nenter reason\ne.g. Annual Maintenance/]
    X --> Y[/Room blocked:\nNo new bookings allowed\nin this period/]
    Y --> D

    D -- Done --> Z([END])
```

---

## 5. AUTHENTICATION FLOW — How Login Works for All Users

```mermaid
flowchart TD
    A([USER OPENS LOGIN PAGE]) --> B[/Enter Windows Username\nEnter Password/]
    B --> C[Click Sign In]
    C --> D{Check local\nIdentity DB}

    D -- Found & password correct --> E[/Signed in via\nlocal Identity/]
    E --> F([REDIRECT TO DASHBOARD])

    D -- Not found or\nwrong password --> G{Check Company Server\nvia CompanyAuthService}

    G -- STUB: Currently returns null --> H[/Show error:\nInvalid username or password/]
    H --> B

    G -- LDAP/REST connected: Credentials valid --> I{Is this user\nnew to this system?}

    I -- Yes: First login --> J[Auto-create local account]
    J --> K[/UserName = windows username\nFullName = from company server\nDepartment = from company server\nRole = User/]
    K --> L[/Signed in/]
    L --> F

    I -- No: Returning user --> M[Update local profile\nfrom company server]
    M --> N[/FullName refreshed\nDepartment refreshed/]
    N --> L

    G -- Credentials invalid --> H
```

---

*Generated for NRL Conference Hall Booking System*
*All four roles (Admin, Allocator, ITFM, User) plus authentication flow*


