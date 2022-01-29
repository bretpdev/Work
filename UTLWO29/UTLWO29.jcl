#UTLWO29.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWO29.LWO29R1
   then
        rm ${reportdir}/ULWO29.LWO29R1
fi
if test -a ${reportdir}/ULWO29.LWO29R2
   then
        rm ${reportdir}/ULWO29.LWO29R2
fi

# run the program

sas ${codedir}/UTLWO29.sas -log ${reportdir}/ULWO29.LWO29R1  -mautosource
