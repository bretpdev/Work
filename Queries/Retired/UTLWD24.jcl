#UTLWD24.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWD24.LWD24R1
   then
        rm ${reportdir}/ULWD24.LWD24R1
fi
if test -a ${reportdir}/ULWD24.LWD24R2
   then
        rm ${reportdir}/ULWD24.LWD24R2
fi

# run the program

sas ${codedir}/UTLWD24.sas -log ${reportdir}/ULWD24.LWD24R1  -mautosource
