#
#
#set environment variables

 . /uheaa/whse/copy_lib/sasprofile
 . /uheaa/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWK01.LWK01R1
   then
        rm ${reportdir}/ULWK01.LWK01R1
fi
if test -a ${reportdir}/ULWK01.LWK01R2
   then
        rm ${reportdir}/ULWK01.LWK01R2
fi

# run the program

sas ${codedir}/UTLWK01.sas -log ${reportdir}/ULWK01.LWK01R1  -mautosource
