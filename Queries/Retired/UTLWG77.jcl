#UTLWG77.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG77.LWG77R1
   then
        rm ${reportdir}/ULWG77.LWG77R1
fi
if test -a ${reportdir}/ULWG77.LWG77R2
   then
        rm ${reportdir}/ULWG77.LWG77R2
fi
if test -a ${reportdir}/ULWG77.LWG77RZ
   then
        rm ${reportdir}/ULWG77.LWG77RZ
fi

# run the program

sas ${codedir}/UTLWG77.sas -log ${reportdir}/ULWG77.LWG77R1  -mautosource
