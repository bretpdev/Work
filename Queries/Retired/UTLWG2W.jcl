#UTLWG2W.jcl	GUI PROVISIONAL APPS NEEDING CERTIFIED
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# run the program

if test -a ${reportdir}/ULWG2W.LWG2WR1
   then
      rm ${reportdir}/ULWG2W.LWG2WR1
fi
if test -a ${reportdir}/ULWG2W.LWG2WR2
   then
      rm ${reportdir}/ULWG2W.LWG2WR2
fi

sas ${codedir}/UTLWG2W.sas -log ${reportdir}/ULWG2W.LWG2WR1  -mautosource
