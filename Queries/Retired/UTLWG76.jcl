#UTLWG76.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG76.LWG76R1
   then
        rm ${reportdir}/ULWG76.LWG76R1
fi
if test -a ${reportdir}/ULWG76.LWG76R2
   then
        rm ${reportdir}/ULWG76.LWG76R2
fi
if test -a ${reportdir}/ULWG76.LWG76RZ
   then
        rm ${reportdir}/ULWG76.LWG76RZ
fi


# run the program

sas ${codedir}/UTLWG76.sas -log ${reportdir}/ULWG76.LWG76R1  -mautosource
