#UTLWG71.jcl     Commonline Apps certified by the school that also have regular apps certified by the borrower
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for CA-Dispatch

if test -a ${reportdir}/ULWG71.LWG71R1
   then
	rm ${reportdir}/ULWG71.LWG71R1
fi
if test -a ${reportdir}/ULWG71.LWG71R2
   then
	rm ${reportdir}/ULWG71.LWG71R2
fi

# run the program
 
sas ${codedir}/UTLWG71.sas -altlog ${reportdir}/ULWG71.LWG71R1  -mautosource
