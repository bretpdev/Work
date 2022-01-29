#UTLWG62.jcl	 Stafford approvals and rejects (by week ending)
#
#Set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

#Delete any existing report files used for CA-Dispatch

if test -a ${reportdir}/ULWG62.LWG62R1
   then
      rm ${reportdir}/ULWG62.LWG62R1
fi
if test -a ${reportdir}/ULWG62.LWG62R2
   then
      rm ${reportdir}/ULWG62.LWG62R2
fi

#Run the program

sas ${codedir}/UTLWG62.sas -log ${reportdir}/ULWG62.LWG62R1  -mautosource
