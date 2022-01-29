#UTLWG10.jcl  hold status report
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG10.LWG10R1
   then
        rm ${reportdir}/ULWG10.LWG10R1
fi
if test -a ${reportdir}/ULWG10.LWG10R2
   then
        rm ${reportdir}/ULWG10.LWG10R2
fi

# run the program

sas ${codedir}/UTLWG10.sas -log ${reportdir}/ULWG10.LWG10R1  -mautosource
