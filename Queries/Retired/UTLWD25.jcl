#UTLWD25.jcl SSA TOP OFFSET OBJECTIONS
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWD25.LWD25R1
   then
        rm ${reportdir}/ULWD25.LWD25R1
fi
if test -a ${reportdir}/ULWD25.LWD25R2
   then
        rm ${reportdir}/ULWD25.LWD25R2
fi
if test -a ${reportdir}/ULWD25.LWD25R3
   then
        rm ${reportdir}/ULWD25.LWD25R3
fi

# run the program

sas ${codedir}/UTLWD25.sas -log ${reportdir}/ULWD25.LWD25R1  -mautosource
