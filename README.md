Vision Detection is a project to researching about Machine Learning application and Azure Architecture. In the current, this design below follow Microservice Architecture and deploy to Azure Cloud.
![System Architecture](https://github.com/TalentVN/VisionDetection/blob/master/docs/Architect%20Design/FlowVisionDetection%20(2).png)

Components:

I. Admin Page: This is page to Admin manage your organization as workplace design, manages:
 1. Work Place settings
 2. Users management according to the work place size.
 3. Machine Learning model settings, training, manage data. Used [https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/](Azure Vision)

II. User Page:
 User can upload your image include your face. This faces will provide as Training data for Vision Model

III. WorkPlace Service:
 Manage Workplace according to the user admin, organization. Setting your Vision Model, security level, schedule training

IV. User Management Service:
 1. User management and User identity

V. Blob Service
 Manage user file upload, process file and communication with other service to share data

VI. Audit Service
 Manage user check-in, check-out time sheet in a day

VII. Vision Service
 Communication with Azure Face API to manage data, training data and detect, identity faces.


