#UTLWBK5.jcl  pull lists for subrogated loans
#
#set environment variables

 . /uheaa/whse/copy_lib/sasprofile
 . /uheaa/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWBK5.LWBK5R1
   then
        rm ${reportdir}/ULWBK5.LWBK5R1
fi
if test -a ${reportdir}/ULWBK5.LWBK5R2
   then
        rm ${reportdir}/ULWBK5.LWBK5R2
fi

# run the program

sas ${codedir}/UTLWBK5.sas -log ${reportdir}/ULWBK5.LWBK5R1  -mautosource
