#UTLWJD5.jcl  pull lists for subrogated loans
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing
if test -a ${reportdir}/ULWJD5.LWJD5R1
   then
        rm ${reportdir}/ULWJD5.LWJD5R1
fi
if test -a ${reportdir}/ULWJD5.LWJD5R2
   then
        rm ${reportdir}/ULWJD5.LWJD5R2
fi

# run the program

sas ${codedir}/UTLWJD5.sas -log ${reportdir}/ULWJD5.LWJD5R1  -mautosource
