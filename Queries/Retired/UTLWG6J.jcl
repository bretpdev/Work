#UTLWG6J.jcl	 Applications with provisional approvals
#
#Set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

#Delete any existing report files used for CA-Dispatch

if test -a ${reportdir}/ULWG6J.LWG6JR1
   then
      rm ${reportdir}/ULWG6J.LWG6JR1
fi
if test -a ${reportdir}/ULWG6J.LWG6JR2
   then
      rm ${reportdir}/ULWG6J.LWG6JR2
fi

#Run the program

sas ${codedir}/UTLWG6J.sas -log ${reportdir}/ULWG6J.LWG6JR1  -mautosource
