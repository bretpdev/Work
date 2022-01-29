#UTLWG52.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG52.LWG52R1
   then
        rm ${reportdir}/ULWG52.LWG52R1
fi
if test -a ${reportdir}/ULWG52.LWG52R2
   then
        rm ${reportdir}/ULWG52.LWG52R2
fi

# run the program

sas ${codedir}/UTLWG52.sas -log ${reportdir}/ULWG52.LWG52R1  -mautosource
