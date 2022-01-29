#UTLWU31.jcl QC - PLUS Credit Denials
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWU31.LWU31R1
then
rm ${reportdir}/ULWU31.LWU31R1
fi
if test -a ${reportdir}/ULWU31.LWU31R2
then
rm ${reportdir}/ULWU31.LWU31R2
fi
if test -a ${reportdir}/ULWU31.LWU31RZ
then
rm ${reportdir}/ULWU31.LWU31RZ
fi

# run the program

sas ${codedir}/UTLWU31.sas -log ${reportdir}/ULWU31.LWU31R1  -mautosource
