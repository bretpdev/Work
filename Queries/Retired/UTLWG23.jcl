#UTLWG23.jcl  provisional apps requiring a PLUS credit check
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG23.LWG23R1
   then
        rm ${reportdir}/ULWG23.LWG23R1
fi
if test -a ${reportdir}/ULWG23.LWG23R2
   then
        rm ${reportdir}/ULWG23.LWG23R2
fi

# run the program

sas ${codedir}/UTLWG23.sas -log ${reportdir}/ULWG23.LWG23R1  -mautosource
