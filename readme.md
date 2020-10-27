# Convert ODataMetadata to OpenAPI Spec

Download the ODATA metadata from D365 from https://<tenant>.crm4.dynamics.com/Tools/SystemCustomization/WsdlProgramming/download.aspx?file=ODataV4Metadata

Correct the path: c:\projects\odata-openapi\ODataV4Metadata.xml to point to the downloaded ODataV4Metadata.xml 

Run the program. 

## Known issues
The filtering of the ODATA schema and paths are are work in progress. Converting a full OData Metadata for a lightly customized CRM instans generates a OpenAPI spec that is larger than 4GB, which is not feasible for anything. 