#UTLWG57.jcl
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWG57.LWG57R1
   then
        rm ${reportdir}/ULWG57.LWG57R1
fi
if test -a ${reportdir}/ULWG57.LWG57R2
   then
        rm ${reportdir}/ULWG57.LWG57R2
fi
if test -a ${reportdir}/ULWG57.LWG57RZ
   then
        rm ${reportdir}/ULWG57.LWG57RZ
fi

# run the program

sas ${codedir}/UTLWG57.sas -log ${reportdir}/ULWG57.LWG57R1  -mautosource
