using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDRCONRPT.Tests
{
    public static class TestData
    {
        public static string NoRejectedBorrowers =
@"O*N05TG71900       ,CLS=RAASPSIN,XXX,BAT=D2017121209523247400000001  ,  00010001               
<?xml version='1.0' encoding='UTF-8'?>
<app:ApplicationActivity xmlns:app='http://www.ed.gov/FSA/Application/v1.4'>
<Borrower>
<TransactionID>B2017121209523535000000001</TransactionID>
<SSN>REDACTED1</SSN>
<Identifiers>
<DateOfBirth>1979-05-28</DateOfBirth>
</Identifiers>
<FullName>
<FirstName>Loyd</FirstName>
<LastName>Ericson</LastName>
<MiddleInitial>I</MiddleInitial>
</FullName>
<Response>
<ResponseCode>Accepted</ResponseCode>
</Response>
</Borrower>
</app:ApplicationActivity>
O*N95TG71900       ,CLS=RAASPSIN,XXX,BAT=D2017121209523247400000001  ,  00020001         ";
        public static string OneRejectedBorrower =
@"O*N05TG71900       ,CLS=RAASPSIN,XXX,BAT=D2017121209523247400000001  ,  00010001               
<?xml version='1.0' encoding='UTF-8'?>
<app:ApplicationActivity xmlns:app='http://www.ed.gov/FSA/Application/v1.4'>
<Borrower>
<TransactionID>B2017121209523535000000001</TransactionID>
<SSN>REDACTED2</SSN>
<Identifiers>
<DateOfBirth>1979-05-28</DateOfBirth>
</Identifiers>
<FullName>
<FirstName>Loyd</FirstName>
<LastName>Ericson</LastName>
<MiddleInitial>I</MiddleInitial>
</FullName>
<Response>
<ResponseCode>Rejected</ResponseCode>
</Response>
</Borrower>
</app:ApplicationActivity>
O*N95TG71900       ,CLS=RAASPSIN,XXX,BAT=D2017121209523247400000001  ,  00020001         ";
        public static string TwoRejectedBorrowers =
@"O*N05TG71900       ,CLS=RAASPSIN,XXX,BAT=D2017121209523247400000001  ,  00010001               
<?xml version='1.0' encoding='UTF-8'?>
<app:ApplicationActivity xmlns:app='http://www.ed.gov/FSA/Application/v1.4'>
<Borrower>
<TransactionID>B2017121209523535000000001</TransactionID>
<SSN>REDACTED3</SSN>
<Identifiers>
<DateOfBirth>1979-05-28</DateOfBirth>
</Identifiers>
<FullName>
<FirstName>Loyd</FirstName>
<LastName>Ericson</LastName>
<MiddleInitial>I</MiddleInitial>
</FullName>
<Response>
<ResponseCode>Rejected</ResponseCode>
</Response>
</Borrower>
<Borrower>
<TransactionID>B2017121209523535000000001</TransactionID>
<SSN>REDACTED4</SSN>
<Identifiers>
<DateOfBirth>1979-05-28</DateOfBirth>
</Identifiers>
<FullName>
<FirstName>Loyd</FirstName>
<LastName>Ericson</LastName>
<MiddleInitial>I</MiddleInitial>
</FullName>
<Response>
<ResponseCode>Rejected</ResponseCode>
</Response>
</Borrower>
</app:ApplicationActivity>
O*N95TG71900       ,CLS=RAASPSIN,XXX,BAT=D2017121209523247400000001  ,  00020001         ";
    }
}
