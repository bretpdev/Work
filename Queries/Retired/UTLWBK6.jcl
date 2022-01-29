#UTLWBK6.jcl  pull lists for subrogated loans
#
#set environment variables

 . /uheaa/whse/copy_lib/sasprofile
 . /uheaa/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWBK6.LWBK6R1
   then
        rm ${reportdir}/ULWBK6.LWBK6R1
fi
if test -a ${reportdir}/ULWBK6.LWBK6R2
   then
        rm ${reportdir}/ULWBK6.LWBK6R2
fi

# run the program

sas ${codedir}/UTLWBK6.sas -log ${reportdir}/ULWBK6.LWBK6R1  -mautosource
